using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnoTel.Web.Cli.Utils;

namespace UnoTel.Web.Cli
{
    public class LoginService
    {
        private readonly CookieProvider _cookieProvider;

        public LoginService(CookieProvider cookieProvider)
        {
            _cookieProvider = cookieProvider ?? throw new System.ArgumentNullException(nameof(cookieProvider));
        }

        internal async Task<string> Login(string userName, string password, int subscriptionNumber)
        {
            string loginContent = string.Empty;

            using (var handler = new HttpClientHandler() { CookieContainer = _cookieProvider.CookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = _cookieProvider.UnoTelBaseAddress })
            {
                var result = await client.GetAsync(string.Empty);
                result.EnsureSuccessStatusCode();
            }

            string subscriptionLink = string.Empty;
            using (var handler = new HttpClientHandler() { CookieContainer = _cookieProvider.CookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = _cookieProvider.UnoTelBaseAddress })
            {
                handler.AllowAutoRedirect = true;

                FormUrlEncodedContent content = CreateLoginForm(userName, password);
                var result = await client.PostAsync("/login.asp", content);

                if (result.StatusCode == HttpStatusCode.Found)
                {
                    subscriptionLink = HandleSingleSubscription(result);
                }
                else
                {
                    result.EnsureSuccessStatusCode();
                    subscriptionLink = HtlmParserUtils.GetSubscriptionNumberFromHtmlString(subscriptionNumber, await result.Content.ReadAsStringAsync());
                }
            }

            using (var handler = new HttpClientHandler() { CookieContainer = _cookieProvider.CookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = _cookieProvider.UnoTelBaseAddress })
            {
                var result = await client.GetAsync(subscriptionLink);
                result.EnsureSuccessStatusCode();
                loginContent = await result.Content.ReadAsStringAsync();
            }

            return loginContent;
        }

        private static string HandleSingleSubscription(HttpResponseMessage result)
        {
            return result.Headers.GetValues("Location").SingleOrDefault();
        }

        private static FormUrlEncodedContent CreateLoginForm(string number, string password)
        {
            return new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("element", "Com_Login_1_0_0"),
                    new KeyValuePair<string, string>("action", "login"),
                    new KeyValuePair<string, string>("com_login_username", number),
                    new KeyValuePair<string, string>("com_login_password", password),
                });
        }
    }
}
