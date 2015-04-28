using OpenQA.Selenium;

namespace WordPressAutomation {
    public class LoginPage {
        public static void GoTo() {
            Driver.Instance.Navigate().GoToUrl("http://localhost:46780/wp-login.php");
        }

        public static LoginCommand LoginAs(string userName) {
            return new LoginCommand(userName);
        }
    }

    public class LoginCommand {
        private readonly string userName;
        private string password;

        public LoginCommand(string userName) {
            this.userName = userName;
        }

        public LoginCommand WithPassword(string password) {
            this.password = password;
            return this;
        }

        public void Login() {
            var loginInput = Driver.Instance.FindElement(By.Id("user_login"));
            loginInput.SendKeys(userName);

            var passwordInput = Driver.Instance.FindElement(By.Id("user_pass"));
            passwordInput.SendKeys(password);

            var loginButton = Driver.Instance.FindElement(By.Id("wp-submit"));
            loginButton.Click();

        }
    }
}
