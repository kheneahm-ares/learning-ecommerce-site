using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Entities
{
    public class ProductBrand : BaseEntity
    {
        public string Name { get; set; }
    }
}