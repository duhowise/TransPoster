using TransPoster.Mvc.Models.Menu;

namespace TransPoster.Mvc.Services;

public interface IMenuService
{
    public List<MenuParentModel> GetMenuList();
}