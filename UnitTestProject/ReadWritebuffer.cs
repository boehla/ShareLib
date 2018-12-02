using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void ReadWriteBufferTest() {

            Lib.ReadWriteBuffer rwb = new Lib.ReadWriteBuffer(5);

            rwb.Write(new byte[] { 0x00, 0x01, 0x02 });

            Assert.AreEqual(3, rwb.Count);

            byte[] dat = rwb.Read(2, false);

            Assert.AreEqual(0x00, dat[0]);
            Assert.AreEqual(0x01, dat[1]);

            Assert.AreEqual(1, rwb.Count);

            rwb.Write(new byte[] { 0x03, 0x04, 0x05 });

            rwb.Read(4, true);
            Assert.AreEqual(4, rwb.Count);

            dat = rwb.Read(4, false);

            Assert.AreEqual(0x02, dat[0]);
            Assert.AreEqual(0x03, dat[1]);
            Assert.AreEqual(0x04, dat[2]);
            Assert.AreEqual(0x05, dat[3]);

            Assert.AreEqual(0, rwb.Count);
            
            try {
                rwb.Read(1, false);
                Assert.Fail("There should be an Exception!!!!");
            } catch { }
        }
    }
}
