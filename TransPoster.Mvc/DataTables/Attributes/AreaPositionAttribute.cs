namespace TransPoster.Mvc.DataTables.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class AreaPositionAttribute : Attribute
{
    public AreaPositionAttribute(int position) => Position = position;

    public AreaPositionAttribute(int position, int positionWithinArea) : this(position)
    {
        PositionWithinArea = positionWithinArea;
    }

    public int Position { get; }
    public int? PositionWithinArea { get; }
}