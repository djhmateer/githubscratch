using OpenQA.Selenium;

namespace WordPressAutomation{
    public class LeftNavigation{
        public class Posts{
            public class AddNew{
                public static void Select(){
                    MenuSelector.Select("menu-posts", "Add New");
                }
            }
        }

        public class Pages{
            public class AllPages{
                public static void Select(){
                    MenuSelector.Select("menu-pages", "All Pages");
                }
            }
        }
    }

    public class MenuSelector{
        public static void Select(string topLevelMenuId, string subMenuLinkText){
            Driver.Instance.FindElement(By.Id(topLevelMenuId)).Click();
            Driver.Instance.FindElement(By.LinkText(subMenuLinkText)).Click();
        }
    }
}