using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Core.Data.Entities
{
    public interface IBaseEntity
    {
        [MaxLength(100)]
        string CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }


        [MaxLength(100)]
        string UpdatedBy { get; set; }
        DateTime UpdatedAt { get; set; }

        DateTime? DeletedAt { get; set; }
    }

    public abstract class BaseEntity<T> : IBaseEntity
    {
        [Key]
        public virtual T Id { get; set; }

        [MaxLength(100)]
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        [MaxLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
