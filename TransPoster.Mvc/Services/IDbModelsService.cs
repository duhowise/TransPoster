namespace TransPoster.Mvc.Services;

public interface IDbModelsService<T> where T : class
{
    public Task<IEnumerable<T>> FindAllAsync();
}