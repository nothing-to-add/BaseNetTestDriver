using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using TechTalk.SpecFlow;

namespace BaseNetTestDriver.Utilities
{
    static class UIDriver
    {
        private static IJavaScriptExecutor jsExecutor;
        private static Actions builder;
        private static Type[] typesToIgnore = new Type[]{
            typeof(ArgumentException),
            typeof(NoSuchElementException),
            typeof(NotImplementedException),
            typeof(InvalidOperationException),
            typeof(ElementNotVisibleException),
            typeof(StaleElementReferenceException)
        };
        private static readonly Destructor Finalise = new Destructor();
        private const int WAIT_SECONDS = 8;

        static UIDriver()
        {
            Initialize();
        }

        private sealed class Destructor
        {
            ~Destructor()
            {
                Browser.Quit();
                KillIEProcess();
            }
        }

        public static void KillIEProcess()
        {
            Process[] processes = Process.GetProcessesByName("IEDriverServer");
            foreach (Process process in processes)
            {
                process.Kill();
            }
        }

        private static void Initialize()
        {
            InternetExplorerOptions options = new InternetExplorerOptions();
            if (FeatureContext.Current.ContainsKey("AuditLog"))
            {
                options.EnableNativeEvents = true;
            }
            else
            {
                options.EnableNativeEvents = false;
            }
            options.AddAdditionalCapability("disable-popup-blocking", true);
            options.EnsureCleanSession = true;
            options.RequireWindowFocus = true;
            options.PageLoadStrategy = PageLoadStrategy.Eager;
            options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;


            Browser = new InternetExplorerDriver(options);

            Browser.Manage().Window.Size = new Size(1024, 768);
            ManageImplicitWait(30);
            jsExecutor = Browser as IJavaScriptExecutor;

            builder = new Actions(Browser);
        }

        public static IWebDriver Browser { get; private set; }

        public static Stopwatch StopWatch { get; set; }

        internal static object ExecuteScript(string script, params object[] args)
        {
            return jsExecutor.ExecuteScript(script, args);
        }

        internal static void GoToUrl(this IWebDriver dd, string url)
        {
            dd.Navigate().GoToUrl(url);
            AddCredentialsToUrl(url);
        }

        internal static void SwitchToNewWindow(this IWebDriver dd, int w = 1)
        {
            dd.SwitchTo().Window(Browser.WindowHandles[w]);
            dd.SwitchTo().Frame(0);
        }

        public static void Restart()
        {
            Browser.Quit();
            Browser.Dispose();
            Browser = null;
            GC.Collect(2, GCCollectionMode.Forced);
            Initialize();
        }

        private static void AddCredentialsToUrl(string url)
        {
            string username = string.Empty;
            string password = string.Empty;

            if (url.Contains(Properties.Settings.Default.WebUrl))
            {
                username = "username";
                password = "password";

            }
            else
            {
                return;
            }

            try
            {
                Thread.Sleep(300);
                Browser.SwitchTo().Alert().SetAuthenticationCredentials(username, password);
                Browser.SwitchTo().Alert().Accept();
                Browser.SwitchTo().DefaultContent();
            }
            catch (NoAlertPresentException Ex) { }// Just consupe exception
        }

        public static void ManageImplicitWait(int seconds = WAIT_SECONDS)
        {
            Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(seconds);
        }

    }
}
