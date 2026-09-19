using System.ComponentModel.DataAnnotations;

namespace Oid85.FinMarket.TraderFinam.Infrastructure.Database.Entities.Base;

public class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
}