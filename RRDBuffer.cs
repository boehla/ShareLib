using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib {
    public class RRDHashBuffer {
        HashSet<long> hashValues;
        long[] arrValues;
        int p;
        public RRDHashBuffer(int length) {
            hashValues = new HashSet<long>();
            arrValues = new long[length];
            p = 0;
        }

        public void addValue(long val) {
            if (val == 0) return; // ignore 0, this is for hashes...
            if (hashValues.Contains(val)) return;

            if (arrValues[p] > 0) hashValues.Remove(arrValues[p]);
            hashValues.Add(val);
            arrValues[p] = val;

            p++;
            if (p >= arrValues.Length) p = 0;
        }
        public bool Contains(long val) {
            return hashValues.Contains(val);
        }
    }
}
