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

    public abstract class BaseEntity : IBaseEntity
    {
        [MaxLength(100)]
        public virtual string CreatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }

        [MaxLength(100)]
        public virtual string UpdatedBy { get; set; }
        public virtual DateTime UpdatedAt { get; set; }

        public virtual DateTime? DeletedAt { get; set; }
    }
}
