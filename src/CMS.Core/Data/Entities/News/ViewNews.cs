using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Core.Data.Entities
{
    [Table("ViewNews")]
    public class ViewNews
    {
        [Key]
        public long Id { get; set; }

        public int Count { get; set; }

        public Guid DetailNewsId { get; set; }
        public DetailNews DetailNews { get; set; }

        public ViewNews() { }
    }
}
