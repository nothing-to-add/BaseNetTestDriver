using BaseNetTestDriver.PageObjects;
using BaseNetTestDriver.Utilities;
using TechTalk.SpecFlow;

namespace BaseNetTestDriver.Steps
{
    [Binding]
    class TheFirstStep
    {
        private TheFirstPage Page = new TheFirstPage();

        public TheFirstStep()
        {
            //constructor if needed
        }

        [Given(@"I navigate to GitHub page")]
        [When(@"I navigate to GitHub page")]
        [Then(@"I navigate to GitHub page")]
        public void WhenINvigateToGitHubPage()
        {
            UIDriver.Browser.Navigate().GoToUrl(Properties.Settings.Default.WebUrl);
        }

        [Given(@"I see the main page")]
        [When(@"I see the main page")]
        [Then(@"I see the main page")]
        public void ISeeTheMainPage()
        {
            Page.GitHubLogo.Displayed.Equals(true);
        }

    }
}
