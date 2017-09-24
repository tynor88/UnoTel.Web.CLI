using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UnoTel.Web.Cli.Utils;
using UnoTel.Web.CLI.Utils;

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
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/min_side/multi/websms.asp")
                {
                    Content = new ByteArrayContent(ContentByteArrayUtils.GetContentByteArray(CreateSmsForm(receiver, message)))
                };
                httpRequestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var result = await client.SendAsync(httpRequestMessage);
                result.EnsureSuccessStatusCode();
                bool smsSent = HtlmParserUtils.SmsHasBeenSent(await result.Content.ReadAsStringAsync());
            }
        }

        private static KeyValuePair<string, string>[] CreateSmsForm(int receiver, string message)
        {
            string encodedMessage = HttpUtility.UrlEncode(message, Encoding.GetEncoding("ISO-8859-1"));

            return new[]
            {
                    new KeyValuePair<string, string>("element", "webSms_1_0_0"),
                    new KeyValuePair<string, string>("action", "SendWebSms"),
                    new KeyValuePair<string, string>("receiver", receiver.ToString()),
                    new KeyValuePair<string, string>("tegn_max", "960"),
                    new KeyValuePair<string, string>("message", encodedMessage)
            };
        }
    }
}
