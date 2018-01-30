namespace UnoTel.Web.Api.DTO.V1
{
    public class Sms
    {
        public int SubscriptionNumber { get; set; }
        public int RecipientNumber { get; set; }
        public string Text { get; set; }
    }
}
