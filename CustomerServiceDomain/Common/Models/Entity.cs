using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Common.Models
{
    public abstract class Entity : IEquatable<Entity>
    {
        public Guid Id { get; protected init; }

        protected Entity(Guid id)
        {
            Id = id;
        }

        protected Entity() { }

        public bool Equals(Entity? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            return Id == other.Id;
        }

        public override bool Equals(object? obj) => Equals(obj as Entity);

        public override int GetHashCode() => Id.GetHashCode();
    }
}
