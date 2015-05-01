using OpenQA.Selenium;
using System;

namespace WordPressAutomation {
    public class NewPostPage {
        public static void GoTo(){
            // Refactor: Should we make a general menu navigation?
            LeftNavigation.Posts.AddNew.Select();
        }

        public static CreatePostCommand CreatePost(string title){
            return new CreatePostCommand(title);
        }

        public static void GoToNewPost(){
            var message = Driver.Instance.FindElement(By.Id("message"));
            var newPostlink = message.FindElements(By.TagName("a"))[0];
            newPostlink.Click();
        }

        public static bool IsInEditMode(){
            //return Driver.Instance.FindElement(By.Id("icon-edit-pages")) != null;
            var h2s = Driver.Instance.FindElements(By.TagName("h2"));
            if (h2s.Count > 0)
                return h2s[0].Text == "Edit Page Add New";

            return false;
        }

        public static string Title{
            get{
                var title = Driver.Instance.FindElement(By.Id("title"));
                if (title != null)
                    return title.GetAttribute("value");
                return string.Empty;
            }
        }
    }

    public class CreatePostCommand{
        private readonly string title;
        private string body;

        public CreatePostCommand(string title){
            this.title = title;
        }

        public CreatePostCommand WithBody(string body) {
            this.body = body;
            return this;
        }

        public void Publish(){
            // type in the title
            Driver.Instance.FindElement(By.Id("title")).SendKeys(title);

            // its actually a separate frame in wordpress
            Driver.Instance.SwitchTo().Frame("content_ifr");
            Driver.Instance.SwitchTo().ActiveElement().SendKeys(body);
            Driver.Instance.SwitchTo().DefaultContent();

            // happens async above, so wait
            Driver.Wait(TimeSpan.FromSeconds(1));

            Driver.Instance.FindElement(By.Id("publish")).Click();
        }
    }
}
