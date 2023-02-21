using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CMS.Core.Domains;

namespace CMS.Core.Data.Entities;

[Table("CategoryNews")]
public class CategoryNews
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    [Column(TypeName = "NVARCHAR")]
    public string Name { get; set; }

    public virtual ICollection<DetailNews> DetailNews { get; set; }

    public CategoryNews() { }

    public CategoryNews(CategoryNewsRequest request)
    {
        Id = request.Id ?? 0;
        Name = request.Name;
    }
}
