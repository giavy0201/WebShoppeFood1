using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace WebMVC.Tests.SeleniumTests
{
    public class TestBase : IDisposable
    {
        protected IWebDriver Driver { get; private set; }

        public TestBase()
        {
            var options = new ChromeOptions();
            // options.AddArgument("--headless"); // Chạy ẩn browser
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            
            Driver = new ChromeDriver(options);
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        }

        public void Dispose()
        {
            Driver?.Quit();
        }
    }
} 