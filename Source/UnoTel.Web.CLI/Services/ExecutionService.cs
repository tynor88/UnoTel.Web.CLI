using System;
using System.Globalization;
using System.Threading.Tasks;
using UnoTel.Web.Cli.Utils;

namespace UnoTel.Web.Cli
{
    public class ExecutionService
    {
        private readonly LoginService _loginService;
        private readonly SendSmsService _sendSmsService;

        public ExecutionService(LoginService loginService, SendSmsService sendSmsService)
        {
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _sendSmsService = sendSmsService ?? throw new ArgumentNullException(nameof(sendSmsService));
        }

        internal async Task SendSMS(string userName, string password, int subscriptionNumber, int recipientNumber, string recipientText)
        {
            await _loginService.Login(userName, password, subscriptionNumber);
            await _sendSmsService.SendSMS(recipientNumber, recipientText);
        }

        internal async Task<decimal> GetBalance(string userName, string password, int subscriptionNumber)
        {
            string loginContent = await _loginService.Login(userName, password, subscriptionNumber);
            string balance = HtlmParserUtils.GetBalanceFromHtmlString(loginContent);
            return decimal.Parse(balance, NumberStyles.AllowDecimalPoint, new CultureInfo("da-DK"));
        }
    }
}
