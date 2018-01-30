using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UnoTel.Web.Api.DTO.V1;
using UnoTel.Web.Core.Services;

namespace UnoTel.Web.Api.Controllers.V1
{
    [Route("v1/[controller]")]
    public class SmsController : Controller
    {
        private readonly IUnoTelService _unoTelService;

        public SmsController(IUnoTelService unoTelService)
        {
            _unoTelService = unoTelService ?? throw new ArgumentNullException(nameof(unoTelService));
        }

        [HttpPost(("send"))]
        public async Task<IActionResult> Post(Sms sms)
        {
            await _unoTelService.SendSMSAsync("test", "test", 123, 123, "test");

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("yey");
        }
    }
}
