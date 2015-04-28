using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordPressAutomation;

namespace WordPressTests {
    [TestClass]
    public class LoginTests {
        [TestInitialize]
        public void Init() {
            Driver.Initialize();
        }


        [TestMethod]
        public void Admin_User_Can_Login() {
            LoginPage.GoTo();
            LoginPage.LoginAs("dave").WithPassword("letmein").Login();

            Assert.IsTrue(DashboardPage.IsAt, "Failed to login");
        }

        [TestCleanup]
        public void CleanUp() {
            Driver.Close();
        }
    }
}
