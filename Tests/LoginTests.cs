using Microsoft.Playwright;
using SauceDemoLogin.Base;

namespace SauceDemoLogin.Tests
{
    [TestFixture]
    public class LoginTests : BaseTest
    {
        [Test]
        [Category("PDJM-3")]
        public async Task LoginExitoso()
        {
            // Navegar a la página de login
            await Page.GotoAsync("https://saucedemo.com");

            // Identificar selectores usernames, password y login button
            var username = Page.GetByPlaceholder("Username");
            await username.FillAsync("standard_user");

            var password = Page.GetByPlaceholder("Password");
            await password.FillAsync("secret_sauce");

            var botonLogin = Page.GetByRole(AriaRole.Button, new() { Name = "Login" });
            await botonLogin.ClickAsync();

            // Validar que el titulo de la pagina sea Products
            var titulo = Page.GetByText("Products");
            await Assertions.Expect(titulo).ToHaveTextAsync("Products");
        }

        [Test]
        [Category("PDJM-4")]
        public async Task UsuarioBloqueado()
        {
            // Navegar a la página de login
            await Page.GotoAsync("https://saucedemo.com");

            // Identificar selectores usernames, password y login button
            var username = Page.GetByPlaceholder("Username");
            await username.FillAsync("locked_out_user");

            var password = Page.GetByPlaceholder("Password");
            await password.FillAsync("secret_sauce");

            var botonLogin = Page.GetByRole(AriaRole.Button, new() { Name = "Login" });
            await botonLogin.ClickAsync();

            // Validar que el login falle y se muestre el mensaje de error
            var mensajeError = Page.GetByText("Epic sadface: Sorry, this user has been locked out.");
            await Assertions.Expect(mensajeError).ToHaveTextAsync("Epic sadface: Sorry, this user has been locked out.");
        }

        [Test]
        [Category("PDJM-4")]        
        public async Task PasswordIncorrecto()
        {
            // Navegar a la página de login
            await Page.GotoAsync("https://saucedemo.com");

            // Identificar selectores usernames, password y login button
            var username = Page.GetByPlaceholder("Username");
            await username.FillAsync("standard_user");

            var password = Page.GetByPlaceholder("Password");
            await password.FillAsync("secret_sauces");

            var botonLogin = Page.GetByRole(AriaRole.Button, new() { Name = "Login" });
            await botonLogin.ClickAsync();

            // Validar que el login falle y se muestre el mensaje de error
            var mensajeError = Page.GetByText("Epic sadface: Username and password do not match any user in this service");
            await Assertions.Expect(mensajeError).ToHaveTextAsync("Epic sadface: Username and password do not match any user in this service");
        }

        [Test]
        [Category("PDJM-4")]
        public async Task ValidarCarrito()
        {
            // Navegar a la página de login
            await Page.GotoAsync("https://saucedemo.com");
            // Identificar selectores usernames, password y login button
            var username = Page.GetByPlaceholder("Username");
            await username.FillAsync("standard_user");
            var password = Page.GetByPlaceholder("Password");
            await password.FillAsync("secret_sauce");
            var botonLogin = Page.GetByRole(AriaRole.Button, new() { Name = "Login" });
            await botonLogin.ClickAsync();

            // Identificar y agregar el primer producto al carrito
            var botonAgregarProducto = Page.Locator("#add-to-cart-sauce-labs-backpack");
            await botonAgregarProducto.ClickAsync();

            // Validar que el carrito tenga 1 producto
            var carrito = Page.Locator("a.shopping_cart_link");
            await Assertions.Expect(carrito).ToHaveTextAsync("1");

        }
    }
}
