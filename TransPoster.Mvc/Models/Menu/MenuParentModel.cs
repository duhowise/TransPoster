namespace TransPoster.Mvc.Models.Menu;

public class MenuParentModel
{
    public string Name { get; set; }
    public string Icon { get; set; }
    public string? Url { get; set; }

    public List<MenuChildModel> Children { get; set; } = new();
}