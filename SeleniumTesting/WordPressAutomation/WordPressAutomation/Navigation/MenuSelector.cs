using OpenQA.Selenium;

namespace WordPressAutomation{
    public class MenuSelector{
        public static void Select(string topLevelMenuId, string subMenuLinkText){
            Driver.Instance.FindElement(By.Id(topLevelMenuId)).Click();
            Driver.Instance.FindElement(By.LinkText(subMenuLinkText)).Click();
        }
    }
}