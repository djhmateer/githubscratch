using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace WordPressAutomation {
    public class ListPostsPage {
        public static void GoTo(PostType postType){
            switch (postType){
                case PostType.Page:
                    Driver.Instance.FindElement(By.Id("menu-pages")).Click();
                    Driver.Instance.FindElement(By.LinkText("All pages")).Click();
                    break;
            }
        }

        public static void SelectPost(string title){
            var postLink = Driver.Instance.FindElement(By.LinkText("Sample Page"));
            postLink.Click();
        }
    }

    public enum PostType{
        Page
    }
}
