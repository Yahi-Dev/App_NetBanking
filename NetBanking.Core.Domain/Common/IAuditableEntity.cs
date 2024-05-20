namespace NetBanking.Core.Domain.Common
{
    public interface IAuditableEntity
    {
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedById { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedById { get; set; }
    }
}
