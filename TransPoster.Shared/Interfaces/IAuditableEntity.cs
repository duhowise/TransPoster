namespace TransPoster.Shared.Interfaces;

public interface IAuditableEntity : IEntity
{
    Guid CreatedUserId { get; set; }
    DateTime CreatedOn { get; set; }
    Guid? ModifiedUserId { get; set; }
    DateTime? ModifiedOn { get; set; }
}
