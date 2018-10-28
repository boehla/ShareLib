using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib {
    public class PowerLib {
        public static PowerDevice pdevice1 = new PowerDevice("B", 5);
        public static PowerDevice pdevice2 = new PowerDevice("B", 6);
        public static PowerDevice pdevice3 = new PowerDevice("B", 4);



        public class PowerDevice {
            private int ON_ID;
            private int OFF_ID;
            public PowerDevice(int off) {
                this.OFF_ID = off;
                this.ON_ID = this.OFF_ID + 1;
            }
            public PowerDevice(int on, int off) {
                this.ON_ID = on;
                this.OFF_ID = off;
            }
            public PowerDevice(string c, int num) {
                this.ON_ID = getID(c, num, true);
                this.OFF_ID = getID(c, num, false);
            }
            public PowerDevice(string c, int group, int num) {
                this.ON_ID = getID(c, group, num, true);
                this.OFF_ID = getID(c, group, num, false);
            }
            public void setDeviceStatus(bool status) {
                Lib.HttpRequest.makeHttpRequestGet(@"http://10.0.1.105/send?rec=" + (status ? this.ON_ID : this.OFF_ID));
            }
            public static int getID(string c, int num, bool on) {
                num = num - 1;
                return getID(c, num / 4, num % 4, on);
            }
                public static int getID(string c, int group, int num, bool on) {
                int ret = 0x000014;

                switch (c.ToUpper()) {
                    case "A": ret |= 0x000000; break;
                    case "B": ret |= 0x400000; break;
                    case "C": ret |= 0x100000; break;
                    case "D": ret |= 0x500000; break;
                    case "E": ret |= 0x040000; break;
                    case "F": ret |= 0x440000; break;
                    case "G": ret |= 0x140000; break;
                    case "H": ret |= 0x540000; break;
                    case "I": ret |= 0x010000; break;
                    case "J": ret |= 0x410000; break;
                    case "K": ret |= 0x110000; break;
                    case "L": ret |= 0x510000; break;
                    case "M": ret |= 0x050000; break;
                    case "N": ret |= 0x450000; break;
                    case "O": ret |= 0x150000; break;
                    case "P": ret |= 0x550000; break;
                }
                switch (group) {
                    case 0: ret |= 0x000000; break;
                    case 1: ret |= 0x000400; break;
                    case 2: ret |= 0x000100; break;
                    case 3: ret |= 0x000500; break;
                }
                switch (num) {
                    case 0: ret |= 0x000000; break;
                    case 1: ret |= 0x004000; break;
                    case 2: ret |= 0x001000; break;
                    case 3: ret |= 0x005000; break;
                }
                if (on) ret |= 1;

                return ret;
            }
        }
    }

    
}
