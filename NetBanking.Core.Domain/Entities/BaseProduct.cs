using NetBanking.Core.Domain.Common;

namespace NetBanking.Core.Domain.Entities
{
    public class BaseProduct : IAuditableEntity
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }

        //Auditable atributes
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedById { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedById { get; set; }        
    }
}
