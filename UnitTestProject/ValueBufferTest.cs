using Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject {
    [TestClass]
    public class ValueBufferTest {
        [TestMethod]
        public void TestValueBuffer() {
            ValueBuffer vb = new ValueBuffer();

            Dictionary<string, object> testValues = new Dictionary<string, object>();
            testValues.Add("strVal", "hoi²³²³1234");
            testValues.Add("intVal", 112312341);
            testValues.Add("doubleVal", double.MaxValue);
            testValues.Add("class", new StringBuilder().Append("asdfa"));

            foreach (KeyValuePair<string, object> item in testValues) {
                Assert.IsNull(vb.getVal(item.Key));
            }
            foreach (KeyValuePair<string, object> item in testValues) {
                vb.setVal(item.Key, item.Value, TimeSpan.FromSeconds(1));
            }
            Assert.AreEqual(testValues.Count, vb.TotalCount);

            foreach (KeyValuePair<string, object> item in testValues) {
                Assert.IsFalse(vb.needsRefresh(item.Key));
                Assert.AreEqual(item.Value, vb.getVal(item.Key));
            }
            DateTime sta = DateTime.UtcNow;
            while(sta.AddSeconds(1) > DateTime.UtcNow) System.Threading.Thread.Sleep(100);

            // Values are still in there, but expired!
            foreach (KeyValuePair<string, object> item in testValues) {
                Assert.IsTrue(vb.needsRefresh(item.Key));
                Assert.AreEqual(item.Value, vb.getVal(item.Key));
            }
            string seckey = "ThisIsANewKey_SoThisNeedsAlawysARefresh";
            Assert.IsTrue(vb.needsRefresh(seckey));
            Assert.IsNull(vb.getVal(seckey));

            vb.setVal(seckey, "value", TimeSpan.FromSeconds(1));
            
            vb.cleanOlds();

            Assert.AreEqual("value", vb.getVal(seckey));
            Assert.AreEqual(1, vb.TotalCount);

            foreach (KeyValuePair<string, object> item in testValues) {
                Assert.IsTrue(vb.needsRefresh(item.Key));
                Assert.IsNull(vb.getVal(item.Key));
            }

            sta = DateTime.UtcNow;
            while (sta.AddSeconds(1) > DateTime.UtcNow) System.Threading.Thread.Sleep(100);

            vb.cleanOlds();

            Assert.AreEqual(0, vb.TotalCount);
        }
    }
}
