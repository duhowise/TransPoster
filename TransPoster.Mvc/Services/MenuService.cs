using TransPoster.Mvc.Models.Menu;

namespace TransPoster.Mvc.Services;

public class MenuService : IMenuService
{
    private readonly List<MenuParentModel> _menu = new()
    {
        new MenuParentModel()
        {
            Name = "Users",
            Icon = "people-fill",
            Url = "Users",
            Children = new ()
            {
                new MenuChildModel()
                {
                    Name = "Create Role",
                    Icon = "person-plus",
                    Url = "Roles/Create"
                }
            }
        },
        new MenuParentModel()
        {
            Name = "Database",
            Icon = "person-badge-fill",
            Url = "/DataExplorer"
        }
    };

    public List<MenuParentModel> GetMenuList()
    {
        return _menu;
    }
}