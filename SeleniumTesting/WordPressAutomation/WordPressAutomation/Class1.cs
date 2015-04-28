using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Firefox;

namespace WordPressAutomation
{
    public class Class1
    {
        public void Go(){
            var driver = new FirefoxDriver();
            //driver.Navigate().GoToUrl("http://google.com");
            driver.Navigate().GoToUrl("http://www.davestopmusic.com");
        }
    }
}
