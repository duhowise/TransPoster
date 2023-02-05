namespace TransPoster.Mvc.Extensions;

public static class ReflectionHelper
{
    public static Type GetNonNullableType(this Type type)
        => IsNullableType(type) ? Nullable.GetUnderlyingType(type) : type;

    public static bool IsNullableType(this Type type)
        => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

    public static bool IsBuiltInSystemTypes(this Type type)
       => type.Namespace == "System";

    public static bool IsNumericType(this Type type)
    {
        var underType = type.GetNonNullableType();

        return Type.GetTypeCode(underType) switch
        {
            TypeCode.Byte
            or TypeCode.SByte
            or TypeCode.UInt16
            or TypeCode.UInt32
            or TypeCode.UInt64
            or TypeCode.Int16
            or TypeCode.Int32
            or TypeCode.Int64
            or TypeCode.Decimal
            or TypeCode.Double
            or TypeCode.Single
            => true,
            _ => false,
        };
    }
}