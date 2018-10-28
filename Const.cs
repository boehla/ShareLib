using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib {
    public class Const {
        static public CultureInfo INV_CULTURE = CultureInfo.InvariantCulture;
        static public double ZERO = 0.00001;
        static public string DATE_TIME_FORMAT = "dd.MM.yyyy_HH:mm:ss";
        static public char CSV_DEL = ';';
        static public DateTime NULL_DATE = new DateTime(10);
        static public DateTime ORIGN_DATE = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        static public int MAX_CHART_POINTS = 1000;

        public class IDS {
            public const string CANDLE_LIST = "CLIST";
            public const string CANDLE_ITEM = "CITEM";
        }
    }
}
