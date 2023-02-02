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
            Url = "/Users",
            Children = new ()
            {
                new MenuChildModel()
                {
                    Name = "Assign Role",
                    Icon = "person-plus",
                    Url = "/Users/AssignRole"
                }
            }
        },
        new MenuParentModel()
        {
            Name = "Roles",
            Icon = "person-badge-fill",
            Url = "/Roles"
        }
    };

    public List<MenuParentModel> GetMenuList()
    {
        return _menu;
    }
}