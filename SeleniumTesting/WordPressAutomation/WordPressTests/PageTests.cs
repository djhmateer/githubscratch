using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordPressAutomation;

namespace WordPressTests {
    [TestClass]
    public class PageTests {
        [TestInitialize]
        public void Init() {
            Driver.Initialize();
        }

        [TestMethod]
        public void Can_Create_A_Basic_Post() {
            LoginPage.GoTo();
            LoginPage.LoginAs("dave").WithPassword("letmein").Login();

            // Pages are just a type of Post - so creating a general purpose class to work with
            // posts and pages, and sending in a type to differentiate
            ListPostsPage.GoTo(PostType.Page);
            ListPostsPage.SelectPost("Sample Page");

            Assert.IsTrue(NewPostPage.IsInEditMode(), "Wasn't in edit mode");
            Assert.AreEqual("Sample Page", NewPostPage.Title, "Title did not match");

        }

        [TestCleanup]
        public void CleanUp() {
            Driver.Close();
        }
    }
}
