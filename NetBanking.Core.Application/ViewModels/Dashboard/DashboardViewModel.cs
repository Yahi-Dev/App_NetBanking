namespace NetBanking.Core.Application.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public string? Id { get; set; }
        public int AllTransaction { get; set; }
        public decimal AllPayments { get; set; }
        public int AllPaymentsToday { get; set; }
        public int AllPaymentsNumber { get; set; }
        public int AssignedProduct { get; set; }
        public int ActiveClients { get; set; }
        public int InactiveClients { get; set; }
    }
}
