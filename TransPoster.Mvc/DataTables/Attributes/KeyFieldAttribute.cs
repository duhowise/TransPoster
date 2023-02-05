using TransPoster.Mvc.DataTables.Model;

namespace TransPoster.Mvc.DataTables.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class KeyFieldAttribute : Attribute
    {
        public KeyFieldAttribute() { }
        public KeyFieldAttribute(OrderDirection orderDirection) => OrderDirection = orderDirection;

        public Model.OrderDirection OrderDirection { get; } = Model.OrderDirection.Asc;
    }
}