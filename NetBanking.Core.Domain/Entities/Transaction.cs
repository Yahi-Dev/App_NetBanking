using NetBanking.Core.Domain.Common;
using NetBanking.Core.Domain.Enums;

namespace NetBanking.Core.Domain.Entities
{
    public class Transaction : IAuditableEntity
    {
        public string EmissorProductId { get; set; }
        public string ReceiverProductId { get; set; }
        public decimal Cantity { get; set; }
        public TransactionType Type { get; set; }

        // Auditable atributes
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedById { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedById { get; set; }
    }
}
