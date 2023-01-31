using TransPoster.Shared.Interfaces;

namespace TransPoster.Data.Models.Shared;

public class AuditableEntity : IAuditableEntity
{
    public virtual Guid Id { get; set; }
    public virtual Guid CreatedUserId { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? ModifiedUserId { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public bool IsActive { get; set; }
}
