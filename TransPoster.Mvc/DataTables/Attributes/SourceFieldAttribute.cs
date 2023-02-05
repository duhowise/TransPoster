namespace TransPoster.Mvc.DataTables.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SourceFieldAttribute : Attribute
{
    #region ctors

    public SourceFieldAttribute() { }

    public SourceFieldAttribute(string name) => Name = name ?? throw new ArgumentNullException(nameof(name));

    public SourceFieldAttribute(params string[] names)
        : this(string.Join(".", names)) { }

    public SourceFieldAttribute(FilterType filterType) => FilterType = filterType;

    public SourceFieldAttribute(FilterType filterType, string[] names) : this(names)
        => FilterType = filterType;

    #endregion // ctors

    public FilterType? FilterType { get; }
    public string Name { get; }
    public string NavigationName { get; set; }
    public bool GlobalSearch { get; set; }
}