using Microsoft.Playwright;

namespace SauceDemoLogin
{
    [SetUpFixture]
    public class PlaywrightSetup
    {
        public static IPlaywright PlaywrightInstance { get; private set; }
        public static IBrowser Browser { get; private set; }

        [OneTimeSetUp]
        public async Task GlobalSetup()
        {
            bool isCI = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"));
            // Inicializar Playwright
            PlaywrightInstance = await Playwright.CreateAsync();
            // Lanzar el navegador Chrome
            Browser = await PlaywrightInstance.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = isCI,
                SlowMo = isCI ? 0 : 1000
            });
        }

        [OneTimeTearDown]
        public async Task GlobalTearDown()
        {
            // Cerrar el navegador y Playwright
            await Browser.CloseAsync();
            PlaywrightInstance.Dispose();
        }
    }
}
