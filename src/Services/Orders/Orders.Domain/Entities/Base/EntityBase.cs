using System.ComponentModel.DataAnnotations.Schema;

namespace Orders.Domain.Entities;

public abstract class EntityBase : IEntityBase {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual Guid Id { get; protected set; } //entity üzerinden id güncellenir sadece

    public EntityBase Clone() {
        return (EntityBase)this.MemberwiseClone();
    }
}