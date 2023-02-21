using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CMS.Core.Domains;
using CMS.Core.Enums;

namespace CMS.Core.Data.Entities;

[Table("DetailNews")]
public class DetailNews
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(250)]
    [Column(TypeName = "NVARCHAR")]
    public string Title { get; set; }

    public string Content { get; set; }

    public DateTime ExpiredDate { get; set; }

    public DetailNewsStatus Status { get; set; }

    public virtual CategoryNews CategoryNews { get; set; }

    public virtual ViewNews ViewNews { get; set; }

    public DetailNews() { }

    public DetailNews(DetailNewsRequest request)
    {
        Title = request.Title;
        Content = request.Content;
        ExpiredDate = request.ExpiredDate;
        Status = request.Status;
    }
}
