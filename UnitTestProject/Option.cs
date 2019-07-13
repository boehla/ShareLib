using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UnitTestProject {
    /// <summary>
    /// Summary description for Option
    /// </summary>
    [TestClass]
    public class Option {
        static string filename = "testfile.json";
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
         [ClassCleanup()]
         public static void MyClassCleanup() {
            if (File.Exists(filename)) File.Delete(filename);
        }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void OptionFileTests() {
            Lib.Options obs = new Lib.Options(filename);

            TestSerialize serl = new TestSerialize();
            for(int i = 0; i < 100; i++) {
                serl.Values.Add(i * i);
            }
            obs.serialize("serl", serl);

            List<TestValue> testvals = new List<TestValue>();
            int val = 1;
            testvals.Add(new TestValue(val++, true));
            testvals.Add(new TestValue(val++, false));
            testvals.Add(new TestValue(val++, 12.123123123m));
            testvals.Add(new TestValue(val++, -12.123123123m));
            testvals.Add(new TestValue(val++, @"Hoi!""$""!)§ &(""$)""$)!,.-()//\\|{}[]<>:;"));
            testvals.Add(new TestValue(val++, "Hoi"));
            testvals.Add(new TestValue(val++, 12));

            foreach (TestValue item in testvals) {
                obs.set(item.name, item.val);
            }

            obs.saveIfNeeded();

            Lib.Options obsNew = new Lib.Options(filename);
            obsNew.load();

            foreach (TestValue item in testvals) {
                if(item.val is bool) {
                    Assert.AreEqual(item.val, obsNew.get(item.name.ToString()).BoolValue);
                } else if(item.val is int) {
                    Assert.AreEqual(item.val, obsNew.get(item.name.ToString()).IntValue);
                } else if (item.val is long) {
                    Assert.AreEqual(item.val, obsNew.get(item.name.ToString()).LongValue);
                } else if (item.val is decimal) {
                    Assert.AreEqual(item.val, obsNew.get(item.name.ToString()).DecimalValue);
                } else if (item.val is string) {
                    Assert.AreEqual(item.val, obsNew.get(item.name.ToString()).Value);
                }
            }

            
            object de = obs.deserialize("serl");
            Assert.IsNotNull(de);
            Assert.IsTrue(de is TestSerialize);
            TestSerialize deser = (TestSerialize)de;
            Assert.AreEqual(serl, deser);
        }

        class TestValue {
            public object name = "";
            public object val = "";
            public TestValue(object name, object val) {
                this.name = name;
                this.val = val;
            }
        }
        [Serializable]
        class TestSerialize {
            public List<int> Values = new List<int>();

            public override bool Equals(object obj) {
                if (obj == null) return false;
                if (!(obj is TestSerialize)) return false;
                TestSerialize ob = (TestSerialize)obj;

                if (ob.Values.Count != this.Values.Count) return false;
                for(int i = 0; i < ob.Values.Count; i++) {
                    if (ob.Values[i] != this.Values[i]) return false;
                }
                return true;
            }
            public override int GetHashCode() {
                int ret = 0;
                for(int i = 0; i < this.Values.Count; i++){
                    ret += (i + 100) * this.Values[i];
                }
                return ret;
            }
        }
    }
}
