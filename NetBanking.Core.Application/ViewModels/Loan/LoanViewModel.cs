namespace NetBanking.Core.Application.ViewModels.Loan
{
    public class LoanViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public decimal Debt { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
