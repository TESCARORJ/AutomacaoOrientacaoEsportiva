
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Globalization;
using System.Text;

class Program
{
    public static string RemoverAcentos(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return texto;

        var normalizedString = texto.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    static async Task Main(string[] args)
    {
        string googleMapsApiKey = "AIzaSyBZsWF92wMD5CEKgF7jD9XkwYudgbU0DrU";
        string searchQuery = "personal trainers rio de janeiro";
        string googleMapsApiUrl = $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={searchQuery}&key={googleMapsApiKey}";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(googleMapsApiUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(responseBody);

            foreach (var result in json["results"])
            {
                string placeId = result["place_id"].ToString();
                string placeDetailsUrl = $"https://maps.googleapis.com/maps/api/place/details/json?place_id={placeId}&key={googleMapsApiKey}";

                HttpResponseMessage detailsResponse = await client.GetAsync(placeDetailsUrl);
                detailsResponse.EnsureSuccessStatusCode();
                string detailsResponseBody = await detailsResponse.Content.ReadAsStringAsync();
                JObject detailsJson = JObject.Parse(detailsResponseBody);

                var resultDetails = detailsJson["result"];
                string name = resultDetails["name"].ToString();

                if (name.Length > 55)
                {
                    name = name.Substring(0, 55);
                }

                string nameEmail = RemoverAcentos(name); // Remover acentos ortográficos
                string[] nameParts = name.Split(' ');

                // Verifica se há pelo menos duas partes para nome e sobrenome
                string firstName = nameParts.Length > 0 ? nameParts[0] : "";
                string lastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";

                string address = resultDetails["formatted_address"].ToString();
                string phoneNumber = resultDetails["formatted_phone_number"] == null ? "" : resultDetails["formatted_phone_number"].ToString();
                string email = string.Join(".", nameEmail.Split(' ')
                    .Select(word => new string(word.Where(c => char.IsLetterOrDigit(c) || c == '.' || c == '-' || c == '_').ToArray()).FirstOrDefault()))
                    + "@orientacaoesportiva.com";

                // Formatar o número de telefone para o formato do WhatsApp
                string formattedPhoneNumber = "55" + phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");

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

      

                // Atualizar Perfil
                driver.FindElement(By.CssSelector("div.user-menu")).Click();
                await Task.Delay(2000);
                driver.FindElement(By.CssSelector("a[href='https://orientacaoesportiva.com/my-profile/']")).Click();
                await Task.Delay(2000);
                driver.FindElement(By.Id("phone")).SendKeys(phoneNumber);
                await Task.Delay(2000);

                if (!string.IsNullOrEmpty(phoneNumber) || phoneNumber != "")
                {
                    driver.FindElement(By.Id("whatsapp")).SendKeys(formattedPhoneNumber);
                }

                await Task.Delay(2000);
                driver.FindElement(By.CssSelector("button[form='edit_user'][type='submit']")).Click();
                await Task.Delay(5000);

                // Adicionar anúncio
                driver.FindElement(By.CssSelector("a.button.border.with-icon")).Click();
                await Task.Delay(5000);

                // Registra Anunciante
                driver.FindElement(By.CssSelector("button[name='continue']")).Click();
                await Task.Delay(5000);

                // Preencher o formulário de anúncio
                var addressField = driver.FindElement(By.Id("_address"));
                addressField.SendKeys(address);
                await Task.Delay(500); // Pequeno atraso para garantir que o valor foi inserido
                addressField.SendKeys(Keys.Enter);

                // Aguarda um curto período para garantir que o endereço foi inserido
                await Task.Delay(2000);

                // Localiza e clica no ícone de marcador de mapa
                //driver.FindElement(By.CssSelector("#_address_wrapper a i.fa-map-marker")).Click();
                //await Task.Delay(2000);


                await Task.Delay(2000);

                driver.FindElement(By.Id("_friendly_address")).SendKeys(address);
                driver.FindElement(By.Id("_phone")).SendKeys(phoneNumber);
                driver.FindElement(By.Id("_email")).SendKeys(email);
                driver.FindElement(By.CssSelector("button[name='submit_listing']")).Click();
                await Task.Delay(5000);

                // Enviar anúncio
                driver.FindElement(By.CssSelector("button[name='continue']")).Click();
                await Task.Delay(5000);

                // Clicar no botão "Inscreva-se agora"
                driver.FindElement(By.Id("billing-first_name")).SendKeys(firstName);
                driver.FindElement(By.Id("billing-last_name")).SendKeys(lastName);
                driver.FindElement(By.Id("billing-address_1")).SendKeys(address);
                driver.FindElement(By.Id("billing-city")).SendKeys("Rio de Janeiro");
                driver.FindElement(By.Id("billing-postcode")).SendKeys("21220-271");

                // Localizar o botão "Inscreva-se agora"
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                var inscrevaSeAgoraButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("button.wc-block-components-checkout-place-order-button.wp-element-button")));

                // Alternativamente, usar JavaScript para clicar
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].scrollIntoView(true);", inscrevaSeAgoraButton);
                await Task.Delay(1000); // Aguarda a rolagem
                js.ExecuteScript("arguments[0].click();", inscrevaSeAgoraButton);
                await Task.Delay(20000);

                driver.Quit();
            }
        }
    }
}
