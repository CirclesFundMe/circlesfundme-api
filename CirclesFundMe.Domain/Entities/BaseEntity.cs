namespace CirclesFundMe.Domain.Entities
{
    public abstract record BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public void DeleteAudit(string deletedBy)
        {
            IsDeleted = true;
            DeletedBy = deletedBy;
            DeletedDate = DateTime.UtcNow;
        }

        public void UpdateAudit(string modifiedBy)
        {
            ModifiedDate = DateTime.UtcNow;
            ModifiedBy = modifiedBy;
        }
    }
}
