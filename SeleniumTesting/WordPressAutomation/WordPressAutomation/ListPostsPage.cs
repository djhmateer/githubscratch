using OpenQA.Selenium;

namespace WordPressAutomation {
    public class ListPostsPage {
        public static void GoTo(PostType postType) {
            switch (postType) {
                case PostType.Page:
                    LeftNavigation.Pages.AllPages.Select();
                   
                    break;
            }
        }

        public static void SelectPost(string title) {
            var postLink = Driver.Instance.FindElement(By.LinkText("Sample Page"));
            postLink.Click();
        }
    }

    public enum PostType {
        Page
    }
}
