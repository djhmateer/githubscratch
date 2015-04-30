using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordPressAutomation;

namespace WordPressTests {
    public class WordpressTest {
        [TestInitialize]
        public void Init() {
            // Singleton to make writing tests easier
            Driver.Initialize();
            LoginPage.GoTo();
            LoginPage.LoginAs("dave").WithPassword("letmein").Login();
        }

        [TestCleanup]
        public void CleanUp() {
            // Using our Driver class, so don't have to talk to IWebDriver directly (which has a Close method)
            Driver.Close();
        }
    }
}
