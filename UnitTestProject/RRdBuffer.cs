using Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject {
    [TestClass]
    public class RRDBuffer {
        [TestMethod]
        public void RRDBufferTest() {

            RRDHashBuffer rRD = new RRDHashBuffer(4);

            for(int i = 0; i < 4; i++) {
                Assert.IsFalse(rRD.Contains(i));
            }
            int testvalue = 1232141;
            rRD.addValue(testvalue);

            for (int i = 0; i < 4; i++) {
                Assert.IsFalse(rRD.Contains(i));
            }

            Assert.IsTrue(rRD.Contains(testvalue));
            rRD.addValue(1);
            rRD.addValue(2);
            rRD.addValue(3);

            Assert.IsTrue(rRD.Contains(testvalue));

            rRD.addValue(4);

            Assert.IsFalse(rRD.Contains(testvalue));
            Assert.IsTrue(rRD.Contains(1));
        }

        [TestMethod]
        public void RRDBufferTestPerformance() {

            RRDHashBuffer rRD = new RRDHashBuffer(100000);

            int last = 0;
            for (int i = 0; i < rRD.Length * 10; i++) {
                rRD.addValue(i);
                last = i;
            }

            Assert.IsFalse(rRD.Contains(1));
            Assert.IsTrue(rRD.Contains(last));
        }
    }
}
