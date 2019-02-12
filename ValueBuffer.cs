using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib {
    public class ValueBuffer {
        /// <summary>
        /// Time for autocleaning the whole buffer; Default 1d;
        /// </summary>
        public TimeSpan AutoCleanTimeSpan { get; set; }
        /// <summary>
        /// Autoclean is enabled by default
        /// </summary>
        public bool AutoCleanEnabled { get; set; }
        /// <summary>
        /// Includes also the expired
        /// </summary>
        public int TotalCount {
            get { return vals.Count; }
        }

        private Dictionary<string, BuffEntry> vals = new Dictionary<string, BuffEntry>();
        private DateTime lastClean = DateTime.UtcNow;
        
        public ValueBuffer() {
            AutoCleanTimeSpan = TimeSpan.FromDays(1);
            AutoCleanEnabled = true;
        }
        public bool needsRefresh(string key) {
            if (!vals.ContainsKey(key) || vals[key].Expired) {
                return true;
            }
            return false;
        }
        public void setVal(string key, object val, DateTime expiredate) {
            if (AutoCleanEnabled && lastClean.Add(AutoCleanTimeSpan) < DateTime.UtcNow) {
                cleanOlds();
            }

            if (!vals.ContainsKey(key)) vals.Add(key, new BuffEntry());
            vals[key].setValue(val, expiredate);
        }
        public void setVal(string key, object val, TimeSpan expire) {
            this.setVal(key, val, DateTime.UtcNow.Add(expire));
        }
        public object getVal(string key) {
            if (!vals.ContainsKey(key)) return null;
            return vals[key].Value;
        }
        public void cleanOlds() {
            List<string> toDel = new List<string>();
            foreach (KeyValuePair<string, BuffEntry> item in this.vals) {
                if (item.Value.Expired) toDel.Add(item.Key);
            }
            foreach (string item in toDel) {
                this.vals.Remove(item);
            }
            lastClean = DateTime.UtcNow;
        }
        public class BuffEntry {
            object value = null;
            DateTime lastSet = new DateTime(0);
            DateTime expire = new DateTime(0);

            public object Value {
                set {
                    this.value = value;
                    lastSet = DateTime.UtcNow;
                }
                get {
                    return value;
                }
            }
            public DateTime LastSet {
                get { return this.lastSet; }
            }
            public DateTime ExpireDate {
                get { return this.expire; }
            }
            public bool Expired {
                get {
                    return this.expire < DateTime.UtcNow;
                }
            }
            public void setValue(object val, DateTime expireDate) {
                this.Value = val;
                this.expire = expireDate;
            }
        }
    }
}
