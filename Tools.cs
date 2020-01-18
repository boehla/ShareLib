using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Lib
{
    public static class JsonExtensions {
        public static bool IsNullOrEmpty(this JToken token) {
            return (token == null) ||
                   (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                   (token.Type == JTokenType.Null);
        }
        public static string formatJObject(JObject ob) {
            return ob.ToString(Newtonsoft.Json.Formatting.None).Replace("\r", "").Replace("\n", "");
        }
    }
    public class Converter
    {
        public static byte[] StringToByteArray(string hex) {
            if (hex.ToLower().StartsWith("0x")) hex = hex.Substring(2);
            byte[] ret =  Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();

            return ret.Reverse().ToArray();
        }
        public static string ByteArrayToString(byte[] ba) {
            return ByteArrayToString(ba, 0, ba.Length);
        }
        public static string ByteArrayToString(byte[] ba, int start, int length) {
            string hex = BitConverter.ToString(ba, start, length);
            return hex.Replace("-", "");
        }
        static public string formatTimeSpanShort(TimeSpan ts) {
            string ret = "";
            if (ts.Days > 0) return ts.Days + "d";
            if (ts.Hours > 0) return ts.Hours + "h";
            if (ts.Minutes > 0) return ts.Minutes + "m";
            if (ts.Seconds > 0) return ts.Seconds + "s";
            if (ts.Milliseconds > 0) return ts.Milliseconds + "ms";
            return ret;
        }

        static public string formatTimeSpan(TimeSpan ts){
            string ret = "";
            if (ts.Days > 0) ret += ts.Days + "d ";
            if (ts.Hours > 0) ret += ts.Hours + "h ";
            if (ts.Minutes > 0) ret += ts.Minutes + "m ";
            if (ts.Seconds > 0) ret += ts.Seconds + "s ";
            if (ts.Milliseconds > 0) ret += ts.Milliseconds + "ms ";
            return ret;
        }
        public static string formatDec(decimal dec) {
            double ddec = (double)Tools.roundDecimal(dec);
            double log = Math.Log10(ddec);
            if (log < 0) log -= 3;
            string surfix = "";
            int pot = (int)log;
            pot /= 3;
            switch (pot) {
                case -4: surfix = "p"; break;
                case -3: surfix = "n"; break;
                case -2: surfix = "u"; break;
                case -1: surfix = "m"; break;
                case 0: surfix = ""; break;
                case 1: surfix = "K"; break;
                case 2: surfix = "M"; break;
                case 3: surfix = "G"; break;
                case 4: surfix = "T"; break;
                case 5: surfix = "P"; break;
            }
            ddec = ddec * Math.Pow(10, -(pot * 3));
            int countdec = 2 - (int)Math.Log10(ddec);
            return string.Format("{0:n" + countdec + "}{1}", ddec, surfix);
        }
        static public string formatHashrate(decimal hashrate) {
            int dec = 0;
            string[] hash = new string[] { "h/s", "Kh/s", "Mh/s", "Gh/s", "Th/s", "Ph/s" };
            while (hashrate > 1000) {
                dec++;
                hashrate /= 1000;
                if (dec >= hash.Length) break;
            }
            return string.Format("{0:0.##}{1}", hashrate, hash[dec]);
        }
        static public string formatSpace(long space) {
            string[] suffix = new string[] { "b", "kb", "mb", "gb", "tb", "pb" };
            int logsize = 0;
            while (space > 1024) {
                logsize++;
                space /= 1024;
            }
            return string.Format("{0}{1}", space, suffix[logsize]);
        }
        static public string toString(Object ob) {
            if (ob == null) return "";
            if (ob is DateTime) {
                DateTime dt = (DateTime)ob;
                return dt.ToString(Const.DATE_TIME_FORMAT);
            }
            return String.Format(CultureInfo.InvariantCulture, "{0}", ob);
        }
        static public bool toBool(Object ob) {
            return toBool(ob, false);
        }
        static public bool toBool(Object ob, bool defValue) {
            if (ob == null) return defValue;
            string strval = ob.ToString();
            bool retvalue = defValue;
            string[] trueValues = new string[] { "1", "true"};
            string[] falseeValues = new string[] { "0", "false" };
            for (int i = 0; i < trueValues.Length; i++) {
                if (strval.Equals(trueValues[i], StringComparison.InvariantCultureIgnoreCase)) return true;
                if (strval.Equals(falseeValues[i], StringComparison.InvariantCultureIgnoreCase)) return false;
            }
            if (bool.TryParse(strval, out retvalue)) return retvalue;
            return defValue;
        }
        static public double toDouble(Object ob){
            return toDouble(ob, 0);
        }
        static public double toDouble(Object ob, double defValue) {
            if (ob == null) return defValue;
            string strval = toString(ob);
            double retvalue = defValue;
            if (double.TryParse(strval, System.Globalization.NumberStyles.Any, Const.INV_CULTURE, out retvalue)) return retvalue;
            return defValue;
        }

        static public int toInt(Object ob) {
            return toInt(ob, 0);
        }
        static public int toInt(Object ob, int defValue) {
            long val = toLong(ob, defValue);
            if (val > int.MaxValue) return defValue;
            if (val < int.MinValue) return defValue;
            return (int)toLong(ob, defValue);
        }
        static public long toLong(Object ob) {
            return toLong(ob, 0);
        }
        static public long toLong(Object ob, long defValue) {
            if (ob == null) return defValue;
            string strval = toString(ob);
            long retvalue = defValue;
            if (long.TryParse(strval, System.Globalization.NumberStyles.Any, Const.INV_CULTURE, out retvalue)) return retvalue;
            double dbvl = 0;
            if (double.TryParse(strval, System.Globalization.NumberStyles.Any, Const.INV_CULTURE, out dbvl)) return (long)dbvl;
            return defValue;
        }
        static public decimal toDecimal(Object ob) {
            return toDecimal(ob, 0);
        }
        static public decimal toDecimal(Object ob, decimal defValue) {
            if (ob == null) return defValue;
            string strval = toString(ob);
            decimal retvalue = defValue;
            if (decimal.TryParse(strval, System.Globalization.NumberStyles.Any, Const.INV_CULTURE, out retvalue)) return retvalue;
            return defValue;
        }
        static public DateTime toDateTime(Object ob) {
            return toDateTime(ob, DateTime.MinValue);
        }
        static public DateTime toDateTime(Object ob, DateTime defValue) {
            return toDateTime(ob, defValue, Const.DATE_TIME_FORMAT);
        }
        static public DateTime toDateTime(Object ob, DateTime defValue, String format){
            if (ob == null) return defValue;
            string strval = toString(ob);
            DateTime retvalue = defValue;
            if (format.ToLower().Equals("1970")) return Const.ORIGN_DATE.AddMilliseconds(Lib.Converter.toDouble(strval) * 1000);
            if (DateTime.TryParseExact(strval, format, Const.INV_CULTURE, DateTimeStyles.None, out retvalue)) return retvalue;
            if (DateTime.TryParse(strval, Const.INV_CULTURE, DateTimeStyles.None, out retvalue)) return retvalue;
            return defValue;
        }
        public static string Base64Encode(byte[] dat) {
            return System.Convert.ToBase64String(dat);
        }
        public static string Base64Encode(string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData) {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static byte[] Base64DecodeBinary(string base64EncodedData) {
            return System.Convert.FromBase64String(base64EncodedData);
        }

        public static bool isEmpty(object ob) {
            if (ob == null) return true;
            if (Lib.Converter.toString(ob).Length <= 0) return true;
            return false;
        }
    }
    public class Tools {
        static public bool checkDoubleEqual(double pa1, double pa2) {
            if (Math.Abs(pa1 - pa2) < Lib.Const.ZERO) return true;
            return false;
        }
        public static long getSeconds1970(DateTime date) {
            return (long)((date - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        }
        public static long getSeconds1970() {
            return getSeconds1970(DateTime.UtcNow);
        }
        public static string explode(string splitter, params object[] pas) {
            StringBuilder sb = new StringBuilder();
            foreach (object item in pas) {
                if (sb.Length > 0) sb.Append(splitter);
                sb.Append(Lib.Converter.toString(item));
            }
            return sb.ToString();
        }
        public static decimal roundDecimal(decimal dec) {
            if (dec == 0m) return dec;
            decimal acc = 0.0001m;
            int pot = 0;
            while (dec * 10m > 1m / acc) {
                dec /= 10m;
                pot++;
            }
            while (dec < 1m / acc) {
                dec *= 10m;
                pot--;
            }
            dec = Math.Round(dec);
            while (pot > 0) {
                dec *= 10m;
                pot--;
            }
            while (pot < 0) {
                dec /= 10m;
                pot++;
            }
            return dec;
        }

        static public string seperateCSV(char sep, params Object[] pars) {
            string ret = "";
            foreach (Object item in pars) {
                ret += Converter.toString(item) + sep;
            }
            return ret;
        }
        static public void makeFolders(string filename) {
            string folder = Path.GetDirectoryName(filename);
            if (!Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }
        }
    }
    public class EtherConvert {
        public static decimal getEtherFromWei(decimal wei) {
            return wei / pow10(18);
        }

        static public decimal pow10(int t) {
            decimal ret = 1;
            for (int i = 0; i < t; i++) {
                ret *= 10;
            }
            return ret;
        }
    }
    public class Performance {
        static private Dictionary<string, MyWatch> watches = new Dictionary<string, MyWatch>();
        static private DateTime Start = DateTime.UtcNow;

        static public void resetWatches() {
            try {
                lock (watches) {
                    watches.Clear();
                    Start = DateTime.UtcNow;
                }
            } catch (Exception ex) {
                Logging.logException("", ex);
            }
        }
        static public void setWatch(string key, bool on) {
            try {
                lock (watches) {
                    int threadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
                    key += "|" + threadID;
                    if (!watches.ContainsKey(key)) watches.Add(key, new MyWatch());
                    if (on) {
                        watches[key].Start();
                    } else {
                        watches[key].Stop();
                    }
                }
            } catch (Exception ex) {
                Logging.logException("", ex);
            }
        }

        static public DataTable getTable() {
            DataTable dt = new DataTable();
            try { 
                dt.Columns.Add("key");
                dt.Columns.Add("per");
                dt.Columns.Add("total");
                dt.Columns.Add("avarage");
                dt.Columns.Add("min");
                dt.Columns.Add("max");
                dt.Columns.Add("last");
                dt.Columns.Add("count");

                Dictionary<string, MyWatch> ret = new Dictionary<string, MyWatch>();
                lock (watches) {
                    List<string> keys = watches.Keys.ToList();
                    keys.Sort();
                    
                    foreach (string key in keys) {
                        if (key == null) continue;
                        MyWatch entry = watches[key];
                        string mainkey = key.Split('|')[0];
                        if (!ret.ContainsKey(mainkey)) ret.Add(mainkey, new MyWatch());
                        ret[mainkey].merge(entry);
                    }
                }
                TimeSpan tot = DateTime.UtcNow - Start;
                foreach(KeyValuePair<string, MyWatch> ent in ret) {
                    DataRow dr = dt.NewRow();
                    dr["key"] = ent.Key;
                    dr["per"] = string.Format("{0:0.0%}", ent.Value.Total.TotalSeconds / tot.TotalSeconds);
                    dr["total"] = formatTimeSpan(ent.Value.Total);
                    dr["avarage"] = formatTimeSpan(ent.Value.Avarage_ms);
                    dr["count"] = string.Format("{0:n0}", ent.Value.Count);
                    dr["min"] = formatTimeSpan(ent.Value.Min);
                    dr["max"] = formatTimeSpan(ent.Value.Max);
                    dr["last"] = formatTimeSpan(ent.Value.Last);
                    dt.Rows.Add(dr);
                }
            } catch (Exception ex) {
                Logging.logException("", ex);
            }
            return dt;
        }

        private class MyWatch : Stopwatch {
            private long count = 0;
            private TimeSpan lastS = TimeSpan.Zero;
            private TimeSpan last = TimeSpan.Zero;
            private TimeSpan min = TimeSpan.MaxValue;
            private TimeSpan max = TimeSpan.MinValue;
            private TimeSpan _elapsed = new TimeSpan(0);
            private long lastStop = 0;
            new public TimeSpan Elapsed {
                set { this._elapsed = value; }
                get { return this._elapsed; }
            }
            public new void Start() {
                if (!base.IsRunning) {
                    count++;
                    lastS = base.Elapsed;
                }
                base.Start();
            }
            public new void Stop() {
                if (base.IsRunning) {
                    last = base.Elapsed - lastS;
                    if (min > last) min = last;
                    if (max < last) max = last;
                    this.Elapsed = base.Elapsed;
                    this.lastStop = DateTime.UtcNow.Ticks;
                }
                base.Stop();
            }
            public double Avarage_ms {
                get {
                    if (count == 0) return 0;
                    return this.Elapsed.TotalMilliseconds / count;
                }
            }
            public TimeSpan Total {
                get { return this.Elapsed; }
            }
            public long Count {
                get { return count; }
            }
            public TimeSpan Last {
                get { return last; }
            }
            public TimeSpan Min {
                get {
                    if (min == TimeSpan.MaxValue) return new TimeSpan(0);
                    return min;
                }
            }
            public TimeSpan Max {
                get {
                    if (max == TimeSpan.MinValue) return new TimeSpan(0);
                    return max;
                }
            }
            public void merge(MyWatch ow) {
                this.count += ow.count;
                if (ow.min < this.min) this.min = ow.min;
                if (ow.max > this.max) this.max = ow.max;
                this.Elapsed = this.Elapsed.Add(ow.Elapsed);
                if(ow.lastStop > this.lastStop) this.last = ow.last;
            }
        }
        static public string formatTimeSpan(double ms) {
            if(ms < 1) {
                return (ms * 1000).ToString("0.00") + "us";
            }
            if (ms < 10) return ms.ToString("0.00") + "ms";
            else return formatTimeSpan(TimeSpan.FromMilliseconds(ms));
        }
        static public string formatTimeSpan(TimeSpan ts) {
            if (ts.Ticks == 0) return "0";
            string ret = "";
            if (ts.Days > 0) ret += ts.Days + "d ";
            if (ts.Hours > 0) ret += ts.Hours + "h ";
            if (ts.Minutes > 0) ret += ts.Minutes + "m ";
            if (ts.Seconds > 0) ret += ts.Seconds + "s ";
            if (ts.Milliseconds > 0) ret += ts.Milliseconds + "ms ";
            return ret;
        }
    }

    public class Options {
        Dictionary<string, OptionParam> values = new Dictionary<string, OptionParam>();
        string filename = "";
        bool changed;

        List<object> uiElements = new List<object>();

        public void addUIElement(object uiel) {
            if (!uiElements.Contains(uiel)) uiElements.Add(uiel);
        }
        bool disableEvents = false;
        public void loadUI() {
            disableEvents = true;
            try {
                foreach (object item in uiElements) {
                    if (item is TextBox) {
                        TextBox tb = (TextBox)item;
                        tb.Text = this.get(tb.Name, tb.Text).Value;
                    } else if (item is CheckBox) {
                        CheckBox cb = (CheckBox)item;
                        cb.Checked = this.get(cb.Name, cb.Checked).BoolValue;
                    } else if (item is NumericUpDown) {
                        NumericUpDown nud = (NumericUpDown)item;
                        nud.Value = this.get(nud.Name, nud.Value).DecimalValue;
                    } else if (item is RadioButton) {
                        RadioButton rb = (RadioButton)item;
                        rb.Checked = this.get(rb.Name, rb.Checked).BoolValue;
                    } else if(item is TrackBar) {
                        TrackBar rb = (TrackBar)item;
                        rb.Value = this.get(rb.Name, rb.Value).IntValue;
                    }
                }
            } finally {
                disableEvents = false;
            }
        }
        public void saveUI() {
            if (disableEvents) return;
            foreach (object item in uiElements) {
                if (item is TextBox) {
                    TextBox tb = (TextBox)item;
                    this.set(tb.Name, tb.Text);
                } else if (item is CheckBox) {
                    CheckBox cb = (CheckBox)item;
                    this.set(cb.Name, cb.Checked);
                } else if (item is NumericUpDown) {
                    NumericUpDown nud = (NumericUpDown)item;
                    this.set(nud.Name, nud.Value);
                } else if (item is RadioButton) {
                    RadioButton rb = (RadioButton)item;
                    this.set(rb.Name, rb.Checked);
                }else if(item is TrackBar) {
                    TrackBar tb = (TrackBar)item;
                    this.set(tb.Name, tb.Value);
                }
            }
        }

        public Options(string filename) {
            this.filename = filename;
        }
        public bool saveIfNeeded() {
            if (changed) return save();
            return true;
        }
        private bool save() {
            try {
                JObject ob = new JObject();
                foreach (KeyValuePair<string, OptionParam> entry in values) {
                    ob.Add(new JProperty(entry.Key, entry.Value.ToString()));
                }
                StreamWriter fout = new StreamWriter(filename);
                fout.Write(ob.ToString());
                fout.Close();
                changed = false;
                return true;
            } catch {
                return false;
            }
            
        }
        public void load() {
            if (!File.Exists(filename)) return;
            StreamReader fin = new StreamReader(filename);
            JObject ob = JObject.Parse(fin.ReadToEnd());
            fin.Close();
            foreach (JProperty item in (JToken)ob) {
                values[item.Name] = new OptionParam(item.Value);
            }
        }
        public void set(object okey, object value) {
            string key = Lib.Converter.toString(okey);
            
            if (!values.ContainsKey(key)) {
                changed = true;
                values[key] = new OptionParam(value);
            } else {
                changed |= values[key].setValue(value);
            }
        }
        IFormatter serialiser = new BinaryFormatter();
        public void serialize(object key, object value) {
            if (!IsSerializable(value)) throw new Exception("Value is not serializable:" + value + "; key=" + key);
            MemoryStream ms = new MemoryStream();
            serialiser.Serialize(ms, value);
            ms.Position = 0;
            byte[] dat = new byte[ms.Length];
            ms.Read(dat, 0, dat.Length);
            this.set(key, Converter.Base64Encode(dat));
        }
        public object deserialize(object key) {
            OptionParam op = this.get(Lib.Converter.toString(key));
            if (op == null) return null;
            MemoryStream ms = new MemoryStream();
            byte[] dat = Converter.Base64DecodeBinary(op.Value);
            ms.Write(dat, 0, dat.Length);
            ms.Position = 0;
            return serialiser.Deserialize(ms);
        }
        public static bool IsSerializable(object obj) {
            Type t = obj.GetType();
            if (t == null) return false;

            return t.IsSerializable;
        }
        public OptionParam get(object key, object def) {
            return get(Lib.Converter.toString(key), def);
        }
        public OptionParam get(string key) {
            if (!values.ContainsKey(key)) return null;
            return values[key];
        }
        public OptionParam get(string key, object def) {
            if (!values.ContainsKey(key)) return new OptionParam(def);
            return values[key];
        }
        public string[] AllKeys {
            get {
                return values.Keys.ToArray();
            }
        }

        public class OptionParam {
            string value = "";

            public OptionParam(object val) {
                this.value = Converter.toString(val);
            }
            public string Value {
                get { return value; }
                set { this.value = value; }
            }

            public int IntValue {
                get { return Converter.toInt(value); }
            }
            public long LongValue {
                get { return Converter.toLong(value); }
            }
            public decimal DecimalValue {
                get { return Converter.toDecimal(value); }
            }
            public bool BoolValue {
                get { return Converter.toBool(value); }
            }
            public bool setValue(object ob) {
                string newvalue = "";
                if (ob == null) {
                    newvalue = "";
                } else {
                    newvalue = Converter.toString(ob);
                }
                bool changed = !newvalue.Equals(value);
                value = newvalue;
                return changed;
            }
            public override string ToString() {
                return value;
            }
        }
    }
    public class MyThreadPool {
        static Dictionary<string, Thread> th = new Dictionary<string, Thread>();

        static public void add(Thread thread) {
            add(thread.Name, thread);
        }
        static public void add(string name, Thread thread) {
            if(thread.Name == null) thread.Name = name;
            if (th.ContainsKey(name)) {
                th[name].Abort();
                th.Remove(name);
            }
            th.Add(name, thread);
        }
        static public void cancelAll() {
            foreach (Thread item in th.Values) {
                item.Abort();
            }
        }
    }
    public class RPCConnection {
        public string rpchost = "localhost";
        public int rpcPort = 18332;
        public string rpclogin = "foo";
        public string rpcpassword = "bar";
        public JObject RequestJObject(string methodName, params JToken[] pars) {
            List<JToken> gentx = new List<JToken>();
            foreach (JToken ob in pars) {
                gentx.Add(ob);
            }
            return JObject.Parse(RequestServer(methodName, gentx));
        }
        public string RequestServer(string methodName, List<JToken> parameters) {
            string responseValue = string.Empty;
            try {
                //Lib.Performance.setWatch("RequestServer_" + methodName, true);
                string ServerIp = "http://" + rpchost + ":" + rpcPort;
                string UserName = rpclogin;
                string Password = rpcpassword;

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ServerIp);
                webRequest.Credentials = new NetworkCredential(UserName, Password);

                webRequest.ContentType = "application/json-rpc";
                webRequest.Method = "POST";

                // Configure request type
                JObject joe = new JObject();
                joe.Add(new JProperty("jsonrpc", "1.0"));
                joe.Add(new JProperty("id", "1"));
                joe.Add(new JProperty("method", methodName));

                JArray props = new JArray();
                foreach (var parameter in parameters) {
                    props.Add(parameter);
                }

                joe.Add(new JProperty("params", props));

                // serialize JSON for request
                string s = JsonConvert.SerializeObject(joe);
                byte[] byteArray = Encoding.UTF8.GetBytes(s);
                webRequest.ContentLength = byteArray.Length;
                webRequest.Timeout = 10000;
                Stream dataStream = webRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                // deserialze the response
                StreamReader sReader = null;
                WebResponse webResponse = webRequest.GetResponse();
                sReader = new StreamReader(webResponse.GetResponseStream(), true);
                responseValue = sReader.ReadToEnd();
            } finally {
                //Lib.Performance.setWatch("RequestServer_" + methodName, false);
            }
            return responseValue;
        }
        public void loadConfigFile(string file) {
            StreamReader fin = new StreamReader(file);
            while (!fin.EndOfStream) {
                string line = fin.ReadLine();
                if (line.StartsWith("#")) continue;
                string[] pars = line.Split('=');
                switch (pars[0]) {
                    case "rpcuser": this.rpclogin = pars[1]; break;
                    case "rpcpassword": this.rpcpassword = pars[1]; break;
                    case "rpcport": this.rpcPort = Lib.Converter.toInt(pars[1]); break;
                }
            }
            fin.Close();
        }
    }

    public class ReadWriteBuffer {
        private readonly byte[] _buffer;
        private int _startIndex, _endIndex;

        public ReadWriteBuffer(int capacity) {
            _buffer = new byte[capacity];
        }

        public int Count {
            get {
                if (_endIndex > _startIndex)
                    return _endIndex - _startIndex;
                if (_endIndex < _startIndex)
                    return (_buffer.Length - _startIndex) + _endIndex;
                return 0;
            }
        }

        public void Write(byte[] data) {
            if (Count + data.Length > _buffer.Length)
                throw new Exception("buffer overflow");
            if (_endIndex + data.Length >= _buffer.Length) {
                var endLen = _buffer.Length - _endIndex;
                var remainingLen = data.Length - endLen;

                Array.Copy(data, 0, _buffer, _endIndex, endLen);
                Array.Copy(data, endLen, _buffer, 0, remainingLen);
                _endIndex = remainingLen;
            } else {
                Array.Copy(data, 0, _buffer, _endIndex, data.Length);
                _endIndex += data.Length;
            }
        }

        public byte[] Read(int len, bool keepData = false) {
            if (len > Count)
                throw new Exception("not enough data in buffer");
            var result = new byte[len];
            if (_startIndex + len < _buffer.Length) {
                Array.Copy(_buffer, _startIndex, result, 0, len);
                if (!keepData)
                    _startIndex += len;
                return result;
            } else {
                var endLen = _buffer.Length - _startIndex;
                var remainingLen = len - endLen;
                Array.Copy(_buffer, _startIndex, result, 0, endLen);
                Array.Copy(_buffer, 0, result, endLen, remainingLen);
                if (!keepData)
                    _startIndex = remainingLen;
                return result;
            }
        }

        public byte this[int index] {
            get {
                if (index >= Count)
                    throw new ArgumentOutOfRangeException();
                return _buffer[(_startIndex + index) % _buffer.Length];
            }
        }

        public IEnumerable<byte> Bytes {
            get {
                for (var i = _startIndex; i < Count; i++)
                    yield return _buffer[(_startIndex + i) % _buffer.Length];
            }
        }
    }
    public class ClassRecycler<T> {
        private T[] buffer;
        int writeind = -1;

        public void put(T newele) {
            lock (this) {
                if (writeind < buffer.Length - 1) {
                    writeind++;
                    buffer[writeind] = newele;
                }
            }
        }
        public T pop() {
            lock (this) {
                if (writeind >= 0) {
                    T ret = buffer[writeind];
                    writeind--;
                    return ret;
                } else {
                    return default(T);
                }
            }
        }
        public ClassRecycler(int capacity) {
            buffer = new T[capacity];
        }
        public int Count {
            get {
                lock (this) {
                    return writeind + 1;
                }
            }
        }
    }
}
