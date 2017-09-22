using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnoTel.Web.Cli
{
    public class SendSmsService
    {
        private readonly CookieProvider _cookieProvider;

        public SendSmsService(CookieProvider cookieProvider)
        {
            _cookieProvider = cookieProvider ?? throw new System.ArgumentNullException(nameof(cookieProvider));
        }

        internal async Task SendSMS(int receiver, string message)
        {
            using (var handler = new HttpClientHandler() { CookieContainer = _cookieProvider.CookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = _cookieProvider.UnoTelBaseAddress })
            {
                var result = await client.GetAsync("/min_side/multi/websms.asp");
                result.EnsureSuccessStatusCode();
            }

            using (var handler = new HttpClientHandler() { CookieContainer = _cookieProvider.CookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = _cookieProvider.UnoTelBaseAddress })
            {
                FormUrlEncodedContent content = CreateSmsForm(receiver, message);
                var result = await client.PostAsync("/min_side/multi/websms.asp", content);
                result.EnsureSuccessStatusCode();
            }
        }

        private static FormUrlEncodedContent CreateSmsForm(int receiver, string message)
        {
            return new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("element", "webSms_1_0_0"),
                    new KeyValuePair<string, string>("action", "SendWebSms"),
                    new KeyValuePair<string, string>("receiver", receiver.ToString()),
                    new KeyValuePair<string, string>("tegn_max", "960"),
                    new KeyValuePair<string, string>("message", message),
                });
        }
    }
}
