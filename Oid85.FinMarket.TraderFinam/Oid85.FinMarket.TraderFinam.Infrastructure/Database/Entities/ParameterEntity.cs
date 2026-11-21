using Oid85.FinMarket.TraderFinam.Infrastructure.Database.Entities.Base;

namespace Oid85.FinMarket.TraderFinam.Infrastructure.Database.Entities
{
    public class ParameterEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
