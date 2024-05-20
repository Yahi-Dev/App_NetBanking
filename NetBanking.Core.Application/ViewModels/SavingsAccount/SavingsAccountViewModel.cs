namespace NetBanking.Core.Application.ViewModels.SavingsAccount
{
    public class SavingsAccountViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public bool IsMain { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
