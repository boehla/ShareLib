using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lib {
    public class TcpTools {
        public class MyTcpServer {
            List<HandleClient> clients = new List<HandleClient>();
            MyMsgBuffer msgRecBuffer = new MyMsgBuffer();
            private int port = 0;
            Thread serthread = null;
            TcpListener serverSocket = null;
            bool running = false;
            public MyTcpServer(int port) {
                this.port = port;

                if (serthread == null || !serthread.IsAlive) {
                    serthread = new Thread(runServer);
                    serthread.IsBackground = true;
                    serthread.Start();
                }
                running = true;
            }
            public int getRecCount() {
                int ret = 0;
                lock (msgRecBuffer) {
                    ret = msgRecBuffer.Count;
                }
                return ret;
            }
            public MyMsg pullRecMsg() {
                MyMsg ret = null;
                lock (msgRecBuffer) {
                    ret = msgRecBuffer.pull();
                }
                return ret;
            }
            public void putRecMsgPriv(MyMsg msg) {
                lock (msgRecBuffer) {
                    msgRecBuffer.put(msg);
                }
            }
            public void putSendMsg(MyMsg msg) {
                List<HandleClient> todel = new List<HandleClient>();
                foreach (HandleClient cl in clients) {
                    if (!cl.Connected) {
                        todel.Add(cl);
                        continue;
                    }
                    if (cl.CustID != null && cl.CustID.Equals(msg.ClientID)) {
                        cl.putSendMsg(msg);
                        Lib.Logging.log("send.txt", string.Format("Send msg to {0}:\n{1}", msg.ClientID, msg.Data));
                    }
                }
                foreach (HandleClient item in todel) {
                    clients.Remove(item);
                }
            }

            private void runServer() {
                while (true) {
                    try {
                        serverSocket = new TcpListener(IPAddress.Any, port);
                        TcpClient clientSocket = default(TcpClient);
                        int counter = 0;

                        try {
                            serverSocket.Start();
                            running = true;
                        }
                        catch {
                            running = false;
                            return;
                        }

                        counter = 0;
                        while (running) {
                            try {
                                counter += 1;
                                clientSocket = serverSocket.AcceptTcpClient();
                                //Logging.log("Client No:" + Convert.ToString(counter) + " started!", LogPrior.Importand);
                                HandleClient client = new HandleClient();
                                this.clients.Add(client);
                                client.startClient(this, clientSocket, Convert.ToString(counter));
                            }
                            catch { }
                        }
                        if (clientSocket != null) clientSocket.Close();
                        if (serverSocket != null) serverSocket.Stop();
                    }
                    catch (Exception ex) {
                        Logging.logException(string.Format("Cannot open server on port {0}", port), ex);
                    }
                }
            }

            

        }


        public class MyTcpClient {
            private static readonly Encoding enc = Encoding.UTF8;
            string host = "";
            int port = 0;
            Thread ctThread = null;
            bool doreconnect = false;
            bool isconnected = false;
            public event OnConnectedHandler OnConnected;
            public delegate void OnConnectedHandler(object source, MyEventArgs e);

            public void stop() {
                running = false;
            }
            public MyTcpClient(string host, int port) {
                this.host = host;
                this.port = port;

                if (ctThread == null) {
                    ctThread = new Thread(runStratum);
                    ctThread.IsBackground = true;
                    ctThread.Start();
                }
                doreconnect = true;
            }
            MyMsgBuffer msgRecBuffer = new MyMsgBuffer();
            MyMsgBuffer msgSendBuffer = new MyMsgBuffer();

            public int getRecCount() {
                int ret = 0;
                lock (msgRecBuffer) {
                    ret = msgRecBuffer.Count;
                }
                return ret;
            }
            public MyMsg pullRecMsg() {
                MyMsg ret = null;
                lock (msgRecBuffer) {
                    ret = msgRecBuffer.pull();
                }
                return ret;
            }
            private void putRecMsg(MyMsg msg) {
                lock (msgRecBuffer) {
                    msgRecBuffer.put(msg);
                }
            }
            public void putSendMsg(MyMsg msg) {
                lock (msgSendBuffer) {
                    msgSendBuffer.put(msg);
                }
            }
            public void clearSendBuffer() {
                lock (msgSendBuffer) {
                    msgSendBuffer.clear();
                }
            }
            private MyMsg pullSendMsg() {
                MyMsg ret = null;
                lock (msgRecBuffer) {
                    ret = msgSendBuffer.pull();
                }
                return ret;
            }
            public bool IsConnected {
                get { return isconnected; }
            }

            bool running = true;
            private void runStratum() {
                TcpClient tcpclient = null;
                StreamReader sin = null;
                StreamWriter sout = null;
                Socket sock = null;
                byte[] buffer = new byte[1024 * 4];
                string recmsg = "";
                DateTime connTime = DateTime.Now;
                while (running) {
                    try {
                        DateTime lastReceive = DateTime.UtcNow;
                        DateTime lastSend = DateTime.UtcNow;
                        TimeSpan timeout = TimeSpan.FromMinutes(2);
                        TimeSpan keepalive = TimeSpan.FromMinutes(1);

                        isconnected = false;
                        tcpclient = new TcpClient(this.host, this.port);
                        sin = new StreamReader(tcpclient.GetStream());
                        sout = new StreamWriter(tcpclient.GetStream());
                        sock = tcpclient.Client;
                        if(OnConnected != null) {
                            OnConnected(this, null);
                        }
                        doreconnect = false;
                        isconnected = true;
                        while (running) {
                            if (sock.Available > 0) {
                                lastReceive = DateTime.UtcNow;
                                int read = sock.Receive(buffer);
                                recmsg += enc.GetString(buffer, 0, read);
                                while (recmsg.Contains("\n")) {
                                    string msg = recmsg.Substring(0, recmsg.IndexOf('\n'));
                                    recmsg = recmsg.Substring(msg.Length + 1);
                                    if (msg.Length <= 1) continue;
                                    MyMsg rec = new MyMsg();
                                    rec.ConvertBase64(msg);
                                    putRecMsg(rec);
                                }
                            }
                            if(lastReceive + timeout < DateTime.UtcNow) {
                                Lib.Logging.log("Timeout.. break connectoin");
                                break; //reconnect
                            }
                            if(lastSend + keepalive < DateTime.UtcNow) {
                                sout.Write("\n");
                                sout.Flush();
                                lastSend = DateTime.UtcNow;
                            }
                            MyMsg outmsg = pullSendMsg();
                            if (outmsg != null) {
                                sout.Write(outmsg.getData() + "\n");
                                sout.Flush();
                            }
                            if (!SocketConnected(sock)) break;
                            if (doreconnect) break;
                        }
                        isconnected = false;
                    }

                    catch {
                    }
                    finally {
                        System.Threading.Thread.Sleep(100);
                    }
                }
                try {
                    sock.Disconnect(false);
                }
                catch { }
            }
        }
        public class MyEventArgs : EventArgs {

        }
        public class HandleClient {
            MyTcpServer srv = null;
            private static readonly Encoding enc = Encoding.UTF8;
            string ip = "";
            TcpClient clientSocket;
            StreamReader sin = null;
            StreamWriter sout = null;
            private bool running = true;
            string clNo;

            MyMsgBuffer msgSendBuffer = new MyMsgBuffer();

            public void putSendMsg(MyMsg msg) {
                lock (msgSendBuffer) {
                    msgSendBuffer.put(msg);
                }
            }

            public string CustID { get; set; }
            public bool Connected {
                get {
                    if (this.clientSocket == null) return false;
                    if (!SocketConnected(clientSocket.Client)) return false;
                    if (!running) return false;
                    return this.clientSocket.Connected;
                }
            }
            public void startClient(MyTcpServer srv, TcpClient inClientSocket, string clineNo) {
                this.srv = srv;
                this.clientSocket = inClientSocket;
                this.clNo = clineNo;
                this.ip = inClientSocket.Client.RemoteEndPoint.ToString();
                sin = new StreamReader(clientSocket.GetStream());
                sout = new StreamWriter(clientSocket.GetStream());
                sout.AutoFlush = true;
                Thread ctThread = new Thread(doClient);
                ctThread.IsBackground = true;
                ctThread.Start();
            }

            private void doClient() {
                running = true;
                Socket sock = clientSocket.Client;
                byte[] buffer = new byte[1024 * 4];
                string recmsg = "";

                DateTime lastReceive = DateTime.UtcNow;
                DateTime lastSend = DateTime.UtcNow;
                TimeSpan timeout = TimeSpan.FromMinutes(2);
                TimeSpan keepalive = TimeSpan.FromMinutes(1);
                while (running) {
                    try {
                        if (sock.Available > 0) {
                            lastReceive = DateTime.UtcNow;
                            int read = sock.Receive(buffer);
                            recmsg += enc.GetString(buffer, 0, read);
                            while (recmsg.Contains("\n")) {
                                string msg = recmsg.Substring(0, recmsg.IndexOf('\n'));
                                recmsg = recmsg.Substring(msg.Length + 1);
                                if (msg.Length <= 1) continue;

                                MyMsg rec = new MyMsg();
                                rec.Client = this;
                                rec.ConvertBase64(msg);
                                rec.ClientID = ip;
                                srv.putRecMsgPriv(rec);
                            }
                        }
                        if (lastReceive + timeout < DateTime.UtcNow) {
                            Lib.Logging.log("Timeout.. break connectoin");
                            break; //reconnect
                        }
                        if (lastSend + keepalive < DateTime.UtcNow) {
                            sout.Write("\n");
                            sout.Flush();
                            lastSend = DateTime.UtcNow;
                        }
                        MyMsg outmsg = msgSendBuffer.pull();
                        if (outmsg != null) {
                            sout.Write(outmsg.getData() + "\n");
                            sout.Flush();
                        }
                        if (!SocketConnected(sock)) break;
                    } catch (Exception ex) {
                        Lib.Logging.logException("", ex);
                    } finally {
                        System.Threading.Thread.Sleep(100);
                    }
                }
                running = false;
            }
        }
        public class MyMsgBuffer {
            List<MyMsg> data = new List<MyMsg>();
            int maxmsgs = 10000;

            public void clear() {
                data.Clear();
            }
            public void put(MyMsg msg) {
                data.Add(msg);
                while (data.Count > maxmsgs) data.RemoveAt(0);
            }
            public MyMsg pull() {
                if(data.Count <= 0) return null;
                MyMsg ret = data[0];
                data.RemoveAt(0);
                return ret;
            }
            public int Count {
                get { return data.Count; }
            }
        }

        public class MyMsg {
            public string ClientID = "";
            public string Data = "";
            public HandleClient Client = null;
            public JObject JData {
                get { return JObject.Parse(Data); }
            }

            public MyMsg() {

            }
            public MyMsg(string text) {
                Data = text;
            }
            public MyMsg(JObject dat) {
                this.Data = dat.ToString();
            }

            public void ConvertBase64(string base64) {
                this.Data = Converter.Base64Decode(base64);
            }
            
            public string getData() {
                return Converter.Base64Encode(Data.ToString());
            }

        }
        static bool SocketConnected(Socket s) {
            bool part1 = s.Poll(5000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }
    }

}
