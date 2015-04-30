using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordPressAutomation;

namespace WordPressTests {
    [TestClass]
    public class CreatePostTests : WordpressTest {
        [TestMethod]
        public void Can_Create_A_Basic_Post() {
            NewPostPage.GoTo();
            NewPostPage.CreatePost("This is a test post title")
                .WithBody("Hi, this is the body")
                .Publish();

            NewPostPage.GoToNewPost();

            Assert.AreEqual(PostPage.Title, "This is a test post title", "Title did not match");
        }
    }
}
