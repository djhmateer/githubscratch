using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;

namespace WordPressAutomation {
    public class Driver {
        public static IWebDriver Instance { get; set; }

        public static string BaseAddress {
            // put in a config file?
            //46780 // work
            //19461 //home
            get { return "http://localhost:19461/"; }
        }

        public static void Initialize() {
            Instance = new FirefoxDriver();
            TurnOnWait();
        }

        public static void Close() {
            Instance.Close();
        }

        public static void Wait(TimeSpan timeSpan) {
            Thread.Sleep((int)timeSpan.TotalSeconds * 1000);
        }

        public static void NoWait(Action action) {
            TurnOffWait();
            action();
            TurnOnWait();
        }

        private static void TurnOnWait() {
            Instance.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
        }

        private static void TurnOffWait() {
            Instance.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }
    }
}