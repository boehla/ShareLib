using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject {
    [TestClass]
    public class Recycler {
        [TestMethod]
        public void RecyclerTest() {

            Lib.ClassRecycler<String> recycler = new Lib.ClassRecycler<String>(5);

            Assert.AreEqual(null, recycler.pop());

            recycler.put("1");
            recycler.put("2");
            recycler.put("3");

            Assert.AreEqual("3", recycler.pop());

            recycler.put("4");
            recycler.put("5");
            recycler.put("6");
            recycler.put("7");
            recycler.put("8");

            Assert.AreEqual("6", recycler.pop());
            Assert.AreEqual("5", recycler.pop());
            Assert.AreEqual("4", recycler.pop());
            Assert.AreEqual("2", recycler.pop());
            Assert.AreEqual("1", recycler.pop());
            Assert.AreEqual(null, recycler.pop());
        }
    }
}
