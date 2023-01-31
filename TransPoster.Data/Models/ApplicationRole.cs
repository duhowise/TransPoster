using Microsoft.AspNetCore.Identity;
using TransPoster.Shared.Interfaces;

namespace TransPoster.Data.Models;

public class ApplicationRole : IdentityRole<Guid>, IAuditableEntity
{
    public Guid CreatedUserId { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? ModifiedUserId { get; set; }
    public DateTime? ModifiedOn { get; set; }
}