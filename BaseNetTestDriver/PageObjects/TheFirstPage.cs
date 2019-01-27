using BaseNetTestDriver.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace BaseNetTestDriver.PageObjects
{
    class TheFirstPage
    {
        public TheFirstPage()
        {
            PageFactory.InitElements(UIDriver.Browser, this);
        }

        /*
         * Locators
         */

        [FindsBy(How = How.CssSelector, Using = "div > a > [class*='octicon-mark']")]
        public IWebElement GitHubLogo { get; set; }

    }
}
