﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using UnoTel.Web.Core.Utils;

namespace UnoTel.Web.Core.Services
{
    public class UnoTelService : IUnoTelService
    {
        private readonly LoginService _loginService;
        private readonly SendSmsService _sendSmsService;

        public UnoTelService(LoginService loginService, SendSmsService sendSmsService)
        {
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _sendSmsService = sendSmsService ?? throw new ArgumentNullException(nameof(sendSmsService));
        }

        public async Task SendSMSAsync(string userName, string password, int subscriptionNumber, int recipientNumber, string recipientText)
        {
            await _loginService.Login(userName, password, subscriptionNumber);
            await _sendSmsService.SendSMS(recipientNumber, recipientText);
        }

        public async Task<decimal> GetBalanceAsync(string userName, string password, int subscriptionNumber)
        {
            string loginContent = await _loginService.Login(userName, password, subscriptionNumber);
            string balance = HtlmParserUtils.GetBalanceFromHtmlString(loginContent);
            return decimal.Parse(balance, NumberStyles.AllowDecimalPoint, new CultureInfo("da-DK"));
        }
    }
}
