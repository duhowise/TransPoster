namespace TransPoster.Mvc.DataTables.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class NavigationSourceAttribute : SourceFieldAttribute
{
    public NavigationSourceAttribute(string name) : base(name) { }

    public NavigationSourceAttribute(params string[] names)
        : base(names)
    { }

    public NavigationSourceAttribute(FilterType filterType, params string[] names)
      : base(filterType, names)
    { }


    public string ValueField { get; set; }
    public string TextField { get; set; }
}