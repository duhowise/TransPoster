namespace TransPoster.Data.Interfaces;

public interface IIdName<TKey> : IKeyed<TKey>
{
    string Name { get; set; }
}

public interface IIdName : IIdName<int>, IKeyed
{
}
