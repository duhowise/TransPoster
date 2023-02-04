using TransPoster.Mvc.Models.Menu;

namespace TransPoster.Mvc.Services;

public class MenuService : IMenuService
{
    private readonly List<MenuParentModel> _menu = new()
    {
        new MenuParentModel()
        {
            Name = "Users",
            Icon = "users",
            Url = "Url",
            Children = new ()
            {
                new MenuChildModel()
                {
                    Name = "Create",
                    Icon = "person-plus",
                    Url = "Users/Create"
                }
            }
        }
    };

    public List<MenuParentModel> GetMenuList()
    {
        return _menu;
    }
}