using System.Reflection;
using TransPoster.Data.Interfaces;
using TransPoster.Mvc.DataTables.Attributes;

namespace TransPoster.Mvc.DataTables.Helpers;

public static class AttributeHelper
{
    public static string GetFilterPath(PropertyInfo property) => GetPath(property, GetFilterPathCore);

    public static string GetOrderPath(PropertyInfo property) => GetPath(property, GetOrderPathCore);

    public static (string name, Model.OrderDirection direction) GetKeyOrder(Type modelType, Type viewModelType)
    {
        var keyProperty = (from p in viewModelType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                           let att = p.GetCustomAttribute<KeyFieldAttribute>()
                           where att != null
                           select new
                           {
                               Property = p,
                               att.OrderDirection
                           }).FirstOrDefault();

        if (keyProperty != null)
        {
            var path = GetFilterPath(keyProperty.Property);
            return (path, keyProperty.OrderDirection);
        }
        else
        {
            if (typeof(IKeyed).IsAssignableFrom(modelType))
            {
                return ("Id", Model.OrderDirection.Asc);
            }
            else
            {
                throw new Exception("Cannot find key to order.");
            }
        }
    }

    private static string? GetPath(PropertyInfo property, Func<PropertyInfo, string> getPathFromProp)
    {
        var path = getPathFromProp(property);
        if (path != null)
        {
            ConcateWithNamespace(ref path, property);
            return path;
        }

        return null;
    }

    private static string GetFilterPathCore(PropertyInfo property)
    {
        var navAttribute = property.GetCustomAttribute<NavigationSourceAttribute>();

        if (navAttribute != null)
        {
            var propName = navAttribute.GetPath(property);
            var valueField = navAttribute.ValueField ?? "Id";
            return $"{propName}.{valueField}";
        }

        var fieldAttribute = property.GetCustomAttribute<SourceFieldAttribute>();

        if (fieldAttribute != null)
        {
            return fieldAttribute.GetPath(property);
        }

        return null;
    }

    private static string GetOrderPathCore(PropertyInfo property)
    {
        var navAttribute = property.GetCustomAttribute<NavigationSourceAttribute>();

        if (navAttribute != null)
        {
            var propName = navAttribute.GetPath(property);
            var textField = navAttribute.TextField ?? "Name";
            return $"{propName}.{textField}";
        }

        var sourceAttribute = property.GetCustomAttribute<SourceFieldAttribute>();

        if (sourceAttribute != null)
        {
            return sourceAttribute.GetPath(property);
        }

        return null;
    }

    private static void ConcateWithNamespace(ref string s, PropertyInfo property)
    {
        if (property.ReflectedType != property.DeclaringType)
        {
            var namespaceAttribute = property.ReflectedType.GetCustomAttributes<NavigationNamespaceAttribute>()
                .FirstOrDefault(att => att.Type == property.DeclaringType);

            if (namespaceAttribute != null)
            {
                s = namespaceAttribute.Namespace + "." + s;
            }

        }
    }

    private static string GetPath(this SourceFieldAttribute sourceField, PropertyInfo property)
    {
        var navigationName = string.IsNullOrEmpty(sourceField.NavigationName) ? "" : sourceField.NavigationName + ".";
        var fieldName = string.IsNullOrEmpty(sourceField.Name) ? property.Name : sourceField.Name;
        return navigationName + fieldName;
    }
}