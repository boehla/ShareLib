﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestProject {
    [TestClass]
    public class ConvertTests {
        [TestMethod]
        public void IntParsingTests() {
            List<object[]> tests = new List<object[]>();
            tests.Add(new object[] { "0", 0 });
            tests.Add(new object[] { "-1", -1 });
            tests.Add(new object[] { "adsf", -1, -1 });
            tests.Add(new object[] { "adsf", 21321, 21321 });
            tests.Add(new object[] { 123d, 123 });
            tests.Add(new object[] { 321f, 321 });
            tests.Add(new object[] { -213L, -213 });
            tests.Add(new object[] { 123.9, 123 });
            tests.Add(new object[] { 123.1, 123 });
            tests.Add(new object[] { "1312 ABCD", 0 });
            tests.Add(new object[] { "1312 ABCD", 10, 10 });
            tests.Add(new object[] { int.MaxValue + "", int.MaxValue });
            tests.Add(new object[] { ((long)int.MaxValue + 1) + "", 0 });
            tests.Add(new object[] { ((long)int.MinValue - 1) + "", 0 });

            foreach (object[] item in tests) {
                if (item.Length >= 3) {
                    Assert.AreEqual(item[1], Lib.Converter.toInt(item[0], (int)item[2]));
                } else {
                    Assert.AreEqual(item[1], Lib.Converter.toInt(item[0]));
                }

            }
        }

        [TestMethod]
        public void LongParsingTests() {
            List<object[]> tests = new List<object[]>();
            tests.Add(new object[] { "0", 0L });
            tests.Add(new object[] { "-1", -1L });
            tests.Add(new object[] { "adsf", -1L, -1L });
            tests.Add(new object[] { "adsf", 21321L, 21321L });
            tests.Add(new object[] { 123d, 123L });
            tests.Add(new object[] { 321f, 321L });
            tests.Add(new object[] { -213L, -213L });
            tests.Add(new object[] { 123.9, 123L });
            tests.Add(new object[] { 123.1, 123L });
            tests.Add(new object[] { "1312 ABCD", 0L });
            tests.Add(new object[] { "1312 ABCD", 10L, 10L });
            tests.Add(new object[] { int.MaxValue + "", (long)int.MaxValue });
            tests.Add(new object[] { ((long)int.MaxValue + 1) + "", (long)int.MaxValue + 1 });
            tests.Add(new object[] { ((long)int.MinValue - 1) + "", (long)int.MinValue - 1 });
            tests.Add(new object[] { (long.MaxValue) + "", long.MaxValue });
            tests.Add(new object[] { (long.MinValue) + "", long.MinValue });

            foreach (object[] item in tests) {
                if (item.Length >= 3) {
                    Assert.AreEqual(item[1], Lib.Converter.toLong(item[0], (long)item[2]));
                } else {
                    Assert.AreEqual(item[1], Lib.Converter.toLong(item[0]));
                }

            }
        }

        [TestMethod]
        public void DecimalParsingTests() {
            List<object[]> tests = new List<object[]>();
            tests.Add(new object[] { "0", 0m });
            tests.Add(new object[] { "-1", -1m });
            tests.Add(new object[] { "adsf", -1m, -1m });
            tests.Add(new object[] { "adsf", 21321m, 21321m });
            tests.Add(new object[] { 123d, 123m });
            tests.Add(new object[] { 321f, 321m });
            tests.Add(new object[] { -213L, -213m });
            tests.Add(new object[] { 123.9, 123.9m });
            tests.Add(new object[] { 123.1, 123.1m });
            tests.Add(new object[] { "1312 ABCD", 0m });
            tests.Add(new object[] { "1312 ABCD", 10m, 10m });
            tests.Add(new object[] { decimal.MaxValue + "", decimal.MaxValue });
            tests.Add(new object[] { decimal.MinValue + "", decimal.MinValue });

            foreach (object[] item in tests) {
                if (item.Length >= 3) {
                    Assert.AreEqual(item[1], Lib.Converter.toDecimal(item[0], (decimal)item[2]));
                } else {
                    Assert.AreEqual(item[1], Lib.Converter.toDecimal(item[0]));
                }

            }
        }

        [TestMethod]
        public void BoolParsingTests() {
            List<object[]> tests = new List<object[]>();
            tests.Add(new object[] { "0", false });
            tests.Add(new object[] { "false", false });
            tests.Add(new object[] { "False", false });
            tests.Add(new object[] { "False", false });
            tests.Add(new object[] { "asdf", false });
            tests.Add(new object[] { "-1", false });
            tests.Add(new object[] { "2", false });

            tests.Add(new object[] { "1", true });
            tests.Add(new object[] { "true", true });
            tests.Add(new object[] { "True", true });
            tests.Add(new object[] { "TRUE", true });

            tests.Add(new object[] { "asdf", true, true });
            tests.Add(new object[] { "asdf", false, false });

            foreach (object[] item in tests) {
                if (item.Length >= 3) {
                    Assert.AreEqual(item[1], Lib.Converter.toBool(item[0], (bool)item[2]));
                } else {
                    Assert.AreEqual(item[1], Lib.Converter.toBool(item[0]));
                }

            }
        }
    }
}
