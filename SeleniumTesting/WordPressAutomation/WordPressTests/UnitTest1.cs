using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordPressAutomation;

namespace WordPressTests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1(){
            var c = new Class1();
            c.Go();
        }
    }
}
