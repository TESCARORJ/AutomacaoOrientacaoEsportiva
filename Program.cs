using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


class Program
{
    static async Task Main(string[] args)
    {
        string googleMapsApiKey = "googleMapsApiKey";
        string searchQuery = "personal trainers são paulo";
        string googleMapsApiUrl = $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={searchQuery}&key={googleMapsApiKey}";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(googleMapsApiUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(responseBody);

            foreach (var result in json["results"])
            {
                string name = result["name"].ToString();
                string address = result["formatted_address"].ToString();
                string phoneNumber = result["formatted_phone_number"] == null ? "" : result["formatted_phone_number"].ToString();
                string email = name.Replace(" ", ".") + "@orientacaoesportiva.com";

                string chromeDriverPath = @"C:\Users\thiago.ntl\Documents\PESSOAL\GT\Automação\AutomacaoOrientacaoEsportiva\chromedriver-win64";
                IWebDriver driver = new ChromeDriver(chromeDriverPath);
                driver.Navigate().GoToUrl("https://orientacaoesportiva.com/");

                // Clicar no botão "Entrar"
                driver.FindElement(By.CssSelector("a.sign-in.popup-with-zoom-anim")).Click();
                await Task.Delay(2000);

                // Clicar na aba "Registrar"
                driver.FindElement(By.CssSelector("li#listeo-registration-btn a[href='#tab2']")).Click();
                await Task.Delay(2000);

                // Preencher o formulário de registro
                driver.FindElement(By.Id("username2")).SendKeys(name);
                driver.FindElement(By.Id("email")).SendKeys(email);
                driver.FindElement(By.Id("password1")).SendKeys("Trocar@123");

                

                // Registra Anunciante
                driver.FindElement(By.CssSelector("input[name='register']")).Click();
                await Task.Delay(5000);

                // Adicionar anúncio
                driver.FindElement(By.CssSelector("a.button.border.with-icon")).Click();
                await Task.Delay(5000);

                // Registra Anunciante
                driver.FindElement(By.CssSelector("button[name='continue']")).Click();
                await Task.Delay(5000);

                // Preencher o formulário de anúncio
                driver.FindElement(By.Id("_address")).SendKeys(address + Keys.Enter);
                driver.FindElement(By.Id("_friendly_address")).SendKeys(address);
                driver.FindElement(By.Id("_phone")).SendKeys(phoneNumber);
                driver.FindElement(By.Id("_email")).SendKeys(email);
                driver.FindElement(By.CssSelector("button[name='submit_listing']")).Click();
                await Task.Delay(5000);

                // Enviar anúncio
                driver.FindElement(By.CssSelector("button[name='continue']")).Click();
                await Task.Delay(5000);

                // Clicar no botão "Inscreva-se agora"
                driver.FindElement(By.CssSelector("div.wc-block-checkout__actions_row button.wc-block-components-checkout-place-order-button")).Click();
                await Task.Delay(5000);

                driver.Quit();
            }
        }
    }

    

}
