using Microsoft.Playwright;
using SauceDemoLogin.Fixtures;

namespace SauceDemoLogin.Base
{
    public class BaseTest
    {
        protected PlaywrightFixture Fixture;
        protected IPage Page;

        [SetUp]
        public async Task Setup()
        {
            // Inicializar el fixture
            Fixture = new PlaywrightFixture();
            // Crear una nueva página para cada prueba
            Page = await Fixture.CrearPageAsync(PlaywrightSetup.Browser);
        }

        [TearDown]
        public async Task TearDown()
        {
            // Cerrar el contexto después de cada prueba
            await Fixture.CerrarContextAsync();
        }
    }
}
