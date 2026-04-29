using Microsoft.Playwright;

namespace SauceDemoLogin.Fixtures
{
    public class PlaywrightFixture
    {
        public IBrowserContext Context { get; private set; } = null!;
        public IPage Page { get; private set; } = null!;

        // Método para crear un nuevo contexto y página
        public async Task<IPage> CrearPageAsync(IBrowser browser)
        {
            // Crear un nuevo contexto
            Context = await browser.NewContextAsync();
            Page = await Context.NewPageAsync();
            return Page;
        }

        // Método para cerrar el contexto
        public async Task CerrarContextAsync()
        {
            await Context.CloseAsync();
        }
    }
}

