namespace NetBanking.Core.Application.ViewModels.CreditCard
{
    public class CreditCardViewModel
    {
        public string? Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public decimal Limit { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
