using OpenQA.Selenium;

namespace WordPressAutomation {
    public class DashboardPage {
        public static bool IsAt {
            get {
                var h2s = Driver.Instance.FindElements(By.TagName("h2"));
                if (h2s.Count > 0)
                    return h2s[0].Text == "Dashboard";
                return false;
            }
        }
    }
}
