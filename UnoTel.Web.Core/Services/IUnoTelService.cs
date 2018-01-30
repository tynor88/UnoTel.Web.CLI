using System.Threading.Tasks;

namespace UnoTel.Web.Core.Services
{
    public interface IUnoTelService
    {
        Task SendSMSAsync(string userName, string password, int subscriptionNumber, int recipientNumber, string recipientText);
        Task<decimal> GetBalanceAsync(string userName, string password, int subscriptionNumber);
    }
}