using Microsoft.EntityFrameworkCore.Metadata;
using TransPoster.Data;
using TransPoster.Mvc.Models.Menu;

namespace TransPoster.Mvc.Services;

public sealed class MenuService : IMenuService
{
    private readonly ApplicationDbContext db;

    public MenuService(ApplicationDbContext db)
    {
        this.db = db ?? throw new ArgumentNullException(nameof(db));
    }


    public List<MenuParentModel> GetMenuList() => new()
    {
        new MenuParentModel()
        {
            Name = "User Management",
            Icon = "people-fill",
            Url = "#",
            Children = new ()
            {
                new MenuChildModel()
                {
                    Name = "Users",
                    Url = "/Users"
                },
                new MenuChildModel()
                {
                    Name = "Roles",
                    Url = "/Roles"
                }
            }
        },
        new MenuParentModel()
        {
            Name = "Database",
            Icon = "person-badge-fill",
            Url = "#",
            Children = GetDbContextTypes()
                .Select(type => new MenuChildModel
            {
                Name = type.ShortName(),
                Url= $@"/DbExplorer?typeName={type.ClrType.AssemblyQualifiedName}",
            }).ToList()
        }
    };

    private IReadOnlyList<IEntityType> GetDbContextTypes()
    {
        // TODO: test it
        return db.Model.GetEntityTypes().ToList();
    }
    
}