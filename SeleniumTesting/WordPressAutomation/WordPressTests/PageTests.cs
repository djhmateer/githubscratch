using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordPressAutomation;

namespace WordPressTests {
    [TestClass]
    public class PageTests : WordpressTest {
        [TestMethod]
        public void Can_Edit_A_Page() {
            // Pages are just a type of Post - so creating a general purpose class to work with
            // posts and pages, and sending in a type to differentiate
            ListPostsPage.GoTo(PostType.Page);
            ListPostsPage.SelectPost("Sample Page");

            Assert.IsTrue(NewPostPage.IsInEditMode(), "Wasn't in edit mode");
            Assert.AreEqual("Sample Page", NewPostPage.Title, "Title did not match");
        }
    }
}
