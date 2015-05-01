using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordPressAutomation;

namespace WordPressTests.PostsTests {
    [TestClass]
    public class AllPostsTest : WordpressTest {
        // can filter by month
        // can filter by category

        // Added posts show up in all posts
        // can trash a post
        // can search posts

        [TestMethod]
        public void Added_Posts_Show_Up() {
            // Go to posts, get post count, store
            ListPostsPage.GoTo(PostType.Posts);
            ListPostsPage.StoreCount();

            // Add a new post
            NewPostPage.GoTo();
            NewPostPage.CreatePost("Added posts show up, title")
                .WithBody("Added posts show up, body")
                .Publish();

            // Check for count
            ListPostsPage.GoTo(PostType.Posts);
            Assert.AreEqual(ListPostsPage.PreviousPostCount + 1, ListPostsPage.CurrentPostCount, "Count is wrong of posts on page");

            // Check for added post
            Assert.IsTrue(ListPostsPage.DoesPostExistWithTitle("Added posts show up, title"));

            // Trash post (clean up)
            ListPostsPage.TrashPost("Added posts show up, title");
            Assert.AreEqual(ListPostsPage.PreviousPostCount, ListPostsPage.CurrentPostCount, "Couldn't trash post");
        }

        [TestMethod]
        public void Can_Search_Posts(){
            // Create new post
            NewPostPage.GoTo();
            NewPostPage.CreatePost("Searching posts, title")
                    .WithBody("Seraching posts, body")
                    .Publish();

            // Go to lists posts
            ListPostsPage.GoTo(PostType.Posts);

            // Search for the post
            ListPostsPage.SearchForPost("Searching posts, title");

            // Check that posts shows up in the results
            Assert.IsTrue(ListPostsPage.DoesPostExistWithTitle("Searching posts, title"));

            // Cleanup (Trash post)
            ListPostsPage.TrashPost("Searching posts, title");
        }
    }
}
