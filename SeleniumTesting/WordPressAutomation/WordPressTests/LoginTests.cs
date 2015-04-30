using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordPressAutomation;

namespace WordPressTests {
    [TestClass]
    public class LoginTests : WordpressTest {
        [TestMethod]
        public void Admin_User_Can_Login() {
            Assert.IsTrue(DashboardPage.IsAt, "Failed to login");
        }
    }
}
