using TransPoster.Mvc.Models.Menu;

namespace TransPoster.Mvc.Services;

public class MenuService : IMenuService
{
    private readonly List<MenuParentModel> _menu = new()
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
            Icon = "database-fill",
            Url = "#",
            Children = new ()
            {
                new MenuChildModel()
                {
                    Name = "Products",
                    Url = "/Products"
                },
            }
        }
    };

    public List<MenuParentModel> GetMenuList()
    {
        return _menu;
    }
}