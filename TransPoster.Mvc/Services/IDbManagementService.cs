namespace TransPoster.Mvc.Services;

public interface IDbManagementService
{
    public List<string?> FindAllTableNamesAsync();
}
