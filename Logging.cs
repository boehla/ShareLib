using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Windows.Shell;
using System.Threading;
using System.Net;

namespace Lib {
    [Flags]
    public enum LogOptions {
        None = 0,
        WithoutDate = 1,
        NotOnUI = 2,
    }
    public class Logging {
        public static readonly string defaultfilename = "logfile.txt";
        public static readonly string  allkey = "log_all.txt";
        private static readonly string _folder = "logs\\";

        private static SharedData shared = new SharedData();
        private static DebugForm form = null;
        private static int _maxFileSize_kb = 10000;
        private static bool first = true;
        private static bool keepRunning = true;

        public static void Close() {
            keepRunning = false;
        }

        public static void log(string text, LogOptions logO = LogOptions.None) {
            log(defaultfilename, text, logO);
        }
        public static void log(string name, string text, LogOptions logO = LogOptions.None) {
            LogEntry loAll = new LogEntry(allkey, text, logO);
            if (!logO.HasFlag(LogOptions.WithoutDate)) loAll.Text = loAll.Text.Insert(0, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + " " + name + " ");

            LogEntry lo = new LogEntry(name, text, logO);
            if (!logO.HasFlag(LogOptions.WithoutDate)) lo.Text = lo.Text.Insert(0, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + " ");

            lock (shared) {
                shared.Logs.put(loAll);
                shared.Logs.put(lo);
            }
            if (first) {
                first = false;
                init();
            }
        }
        private static void init() {
            form = new DebugForm();
            Thread logth = new Thread(logThread);
            logth.IsBackground = true;
            logth.Start();
            MyThreadPool.add("logger", logth);
        }
        private static void logThread() {
            while (keepRunning) {
                try {
                    LogEntry clog = null;
                    do {
                        lock (shared) {
                            clog = shared.Logs.pull();
                        }
                        if (clog == null) break;
                        logMainThread(clog);
                    } while (clog != null);
                } catch {
                } finally {
                    Thread.Sleep(100);
                }
            }
        }
        private static void logMainThread(LogEntry loge) {
            try {
                string filename = loge.Filename;
                string text = loge.Text.ToString();
                DateTime dt = DateTime.Now;

                if (!text.EndsWith("\r\n")) text += "\r\n";
                if(!loge.Flags.HasFlag(LogOptions.NotOnUI)) form.log(loge);
                
                writeToLogFile(text, filename);
            } catch { }
        }
        public static void logException(string text, Exception ex) {
            logException(defaultfilename, text, ex);
        }
        public static void logException(string file, string text, Exception ex) {
            try {
                try {
                    if (ex is WebException) {
                        StreamReader sReader = null;
                        WebResponse webResponse = ((WebException)ex).Response;
                        if (webResponse != null) {
                            sReader = new StreamReader(webResponse.GetResponseStream(), true);
                            string responseValue = sReader.ReadToEnd();
                            text += "\r\n" + responseValue;
                        }
                    }
                    if(ex.InnerException != null) {
                        text += ex.InnerException.Message + "\r\n" + ex.InnerException.StackTrace;
                    }
                } catch { }

                text += ex.Message + "\r\n" + ex.StackTrace;
                log(file, text);
            } catch { }
        }
        public static void showForm() {
            if (form == null) init();
            form.showForm();
            form.BringToFront();
            form.WindowState = FormWindowState.Normal;
        }

        private static void writeToLogFile(string text, string filename) {
            try {
                string logfile = _folder + filename;
                string createfolder = new FileInfo(logfile).Directory.FullName;
                if (!Directory.Exists(createfolder)) {
                    Directory.CreateDirectory(createfolder);
                }

                if (File.Exists(logfile)) {
                    FileInfo finfo = new FileInfo(logfile);
                    if (finfo.Length > _maxFileSize_kb * 1024) {
                        string oldLog = logfile + ".old";
                        if (File.Exists(oldLog)) File.Delete(oldLog);
                        File.Move(logfile, oldLog);
                    }
                }
                StreamWriter fout = new StreamWriter(logfile, true);
                fout.Write(text);
                fout.Close();
            } catch { }
        }
        class SharedData {
            public LogEntries Logs = new LogEntries();
        }
    }
    public class LogEntries {
        private Dictionary<string, LogEntry> data { get; set; }

        public LogEntries() {
            this.data = new Dictionary<string, LogEntry>();
        }
        public void put(LogEntry en) {
            if (this.data.ContainsKey(en.Filename)) this.data[en.Filename].Text.Append("\r\n").Append(en.Text);
            else this.data[en.Filename] = en;
        }
        public LogEntry pull() {
            if (this.data.Count <= 0) return null;
            LogEntry ret = this.data[this.data.Keys.ToArray()[0]];
            this.data.Remove(ret.Filename);
            return ret;
        }
        public int Count {
            get { return this.data.Count; }
        }
        public Dictionary<string, LogEntry> Items {
            get { return this.data; }
        }
        public void ClearTexts() {
            foreach (LogEntry item in data.Values) {
                item.Text.Clear();
            }
        }
    }
    public class LogEntry {
        public string Filename { get; set; }
        public StringBuilder Text { get; set; }
        public LogOptions Flags { get; set; }

        public LogEntry(string name, string text, LogOptions logO) {
            this.Filename = name;
            this.Text = new StringBuilder(text);
            this.Flags = logO;
        }
    }
}
