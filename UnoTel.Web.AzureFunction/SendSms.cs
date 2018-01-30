using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using UnoTel.Web.Api.DTO.V1;
using UnoTel.Web.Core.Services;
using System.Configuration;

namespace UnoTel.Web.AzureFunction
{
    public static class SendSms
    {
        [FunctionName("SendSms")]
        public static async System.Threading.Tasks.Task<HttpResponseMessage> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/sms")]HttpRequestMessage req, TraceWriter log)
        {
            Sms smsData = await req.Content.ReadAsAsync<Sms>();

            var userName = ConfigurationManager.AppSettings["unoTelUserName"];
            var password = ConfigurationManager.AppSettings["unoTelPassword"];

            log.Info($"UnoTel UserName: {userName} | password: {password} | subscriptionNumber: {smsData.SubscriptionNumber}");
            log.Info($"Sending SMS {smsData.SubscriptionNumber} -> {smsData.RecipientNumber} with content: {smsData.Text}");

            CookieProvider cookieProvider = new CookieProvider();

            IUnoTelService unoTelService = new UnoTelService(new LoginService(cookieProvider), new SendSmsService(cookieProvider));
            await unoTelService.SendSMSAsync(userName, password, smsData.SubscriptionNumber, smsData.RecipientNumber, smsData.Text);

            log.Info($"Sent SMS {smsData.SubscriptionNumber} -> {smsData.RecipientNumber} with content: {smsData.Text}");
            return req.CreateResponse(HttpStatusCode.OK, $"Sent SMS {smsData.SubscriptionNumber} -> {smsData.RecipientNumber} with content: {smsData.Text}");
        }
    }
}
