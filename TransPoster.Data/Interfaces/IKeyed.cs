namespace TransPoster.Data.Interfaces;

public interface IKeyed<TKey>
{
    TKey Id { get; set; }
}

public interface IKeyed : IKeyed<int>
{
}