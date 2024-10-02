using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Abstractions
{
    public class Entity<T> : IEntity<T>
    {
        public T Id { get; set; } = default!;
        public DateTime? CreatedAt { get; set; } = default!;
        public string? CreatedBy { get; set; } = default!;
        public DateTime? LastModified { get ; set ; } = default!;
        public string? LastModifiedBy { get ; set ; } = default!;
    }
}
