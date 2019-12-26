using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib {
    public class LRRD {
        private int p = 0;
        private LRRDEntry[] values;
        private DateTime lastDate;
        private TimeSpan Resolution;
        private LRRDEntry TotalValue;

        public LRRD(TimeSpan Length, TimeSpan Resolution) {
            this.Resolution = Resolution;
            TotalValue = new LRRDEntry();
            int l = (int)(Length.TotalSeconds / Resolution.TotalSeconds);
            values = new LRRDEntry[l];
            for(int i = 0; i < values.Length; i++) {
                values[i] = new LRRDEntry();
            }
            lastDate = DateTime.Now;
        }
        public void addValue(double value, double weight, DateTime date) {
            shiftToDate(date);
            int curp = (int)((lastDate.Ticks - date.Ticks) / Resolution.Ticks);
            if (curp >= values.Length) return; // out of range
            curp = (p + curp) % values.Length;
            values[curp].add(value, weight);
            TotalValue.add(value, weight);
        }
        public void shiftToDate(DateTime date) {
            while (lastDate < date) {
                p = (p + 1) % values.Length;
                TotalValue.sub(values[p]);
                values[p].reset();
                lastDate += Resolution;
            }
        }
        public double getValue() {
            shiftToDate(DateTime.Now);
            return TotalValue.Value;
        }
        public double getTotValue() {
            shiftToDate(DateTime.Now);
            return TotalValue.TotalValue;
        }
        public double getPerValue(TimeSpan sp) {
            shiftToDate(DateTime.Now);
            return TotalValue.TotalValue / (Resolution.TotalSeconds * values.Length) * sp.TotalSeconds;
        }
        class LRRDEntry {
            private double weight;
            private double totval;

            public LRRDEntry() {
                this.reset();
            }

            public void reset() {
                this.weight = 0;
                this.totval = 0;
            }
            public void add(double value, double weight) {
                this.weight += weight;
                this.totval += value * weight;
            }
            public void add(double value) {
                this.add(value, 1);
            }

            public double Value {
                get { if (weight <= 0.0000001) return 0;
                    return totval / weight; }
            }
            public double TotalValue {
                get { return totval; }
            }
            public double Weight {
                get { return weight; }
            }

            public void sub(LRRDEntry entry) {
                this.totval -= entry.totval;
                this.weight -= entry.weight;
            }
            public override string ToString() {
                return string.Format("{0} ({1})", this.Value, this.weight);
            }
        }

    }
    
}
