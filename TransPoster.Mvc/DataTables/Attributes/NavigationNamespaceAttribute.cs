namespace TransPoster.Mvc.DataTables.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
public sealed class NavigationNamespaceAttribute : Attribute
{
    public NavigationNamespaceAttribute(Type type, string @namespace)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Namespace = @namespace ?? throw new ArgumentNullException(nameof(@namespace));
    }

    public Type Type { get; }
    public string Namespace { get; }
}