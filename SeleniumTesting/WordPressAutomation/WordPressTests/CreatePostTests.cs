using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordPressAutomation;

namespace WordPressTests {
    [TestClass]
    public class CreatePostTests {
        [TestInitialize]
        public void Init() {
            Driver.Initialize();
        }

        [TestMethod]
        public void Can_Create_A_Basic_Post() {
            LoginPage.GoTo();
            LoginPage.LoginAs("dave").WithPassword("letmein").Login();

            NewPostPage.GoTo();
            NewPostPage.CreatePost("This is a test post title")
                .WithBody("Hi, this is the body")
                .Publish();

            NewPostPage.GoToNewPost();

            Assert.AreEqual(PostPage.Title, "This is a test post title", "Title did not match");
        }

        [TestCleanup]
        public void CleanUp() {
            Driver.Close();
        }
    }
}
