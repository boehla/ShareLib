using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib {
    public class RRDHashBuffer {
        public long Length {
            get { return arrValues.Length; }
        }

        HashSet<long> hashValues;
        long[] arrValues;
        int p;
        public RRDHashBuffer(int length) {
            hashValues = new HashSet<long>();
            arrValues = new long[length];
            p = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns>returns false if not added, because already contains value</returns>
        public bool addValue(long val) {
            if (val == 0) return false; // ignore 0, this is for hashes...
            if (hashValues.Contains(val)) return false;

            if (arrValues[p] != 0) hashValues.Remove(arrValues[p]);
            hashValues.Add(val);
            arrValues[p] = val;

            p++;
            if (p >= arrValues.Length) p = 0;
            return true;
        }
        public bool Contains(long val) {
            return hashValues.Contains(val);
        }

    }
}
