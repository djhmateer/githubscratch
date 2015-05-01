using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace WordPressAutomation {
    public class ListPostsPage {
        // Static field
        private static int lastCount;
        // Property
        public static int PreviousPostCount {
            get { return lastCount; }
        }

        public static int CurrentPostCount{
            get { return GetPostCount(); }
        }

        public static void GoTo(PostType postType) {
            switch (postType) {
                case PostType.Page:
                    LeftNavigation.Pages.AllPages.Select();
                    break;
                case PostType.Posts:
                    LeftNavigation.Posts.AllPosts.Select();
                    break;
            }
        }

        public static void SelectPost(string title) {
            var postLink = Driver.Instance.FindElement(By.LinkText("Sample Page"));
            postLink.Click();
        }

        public static void StoreCount() {
            // static variable
            lastCount = GetPostCount();
        }

        private static int GetPostCount() {
            var countText = Driver.Instance.FindElement(By.ClassName("displaying-num")).Text;
            return int.Parse(countText.Split(' ')[0]);
        }

        public static bool DoesPostExistWithTitle(string title){
            return Driver.Instance.FindElements(By.LinkText(title)).Any();
        }

        public static void TrashPost(string title){
            // find all rows
            var rows = Driver.Instance.FindElements(By.TagName("tr"));
            foreach (var row in rows) {
                ReadOnlyCollection<IWebElement> links = null;
                
                Driver.NoWait(() => links = row.FindElements(By.LinkText(title)));

                // it will wait for 5s.. maybe ajax?
                //links = row.FindElements(By.LinkText(title));

                if (links.Count > 0) {
                    Actions action = new Actions(Driver.Instance);
                    // Doing the hover over
                    action.MoveToElement(links[0]);
                    action.Perform();
                    row.FindElement(By.ClassName("submitdelete")).Click();
                    return;
                }
            }
        }

        public static void SearchForPost(string searchString){
            //if (!IsAt)
            //    GoTo(PostType.Posts);

            var searchBox = Driver.Instance.FindElement(By.Id("post-search-input"));
            searchBox.SendKeys(searchString);

            var searchButton = Driver.Instance.FindElement(By.Id("search-submit"));
            searchButton.Click();
        }
    }

    public enum PostType {
        Page,
        Posts
    }
}
