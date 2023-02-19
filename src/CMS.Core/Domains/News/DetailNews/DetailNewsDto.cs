using CMS.Core.Data.Entities;
using CMS.Core.Enums;
using System;

namespace CMS.Core.Domains
{
    public class DetailNewsDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime ExpiredDate { get; set; }

        public DetailNewsStatus Status { get; set; }

        public int? CategoryNewsId { get; set; }
        public CategoryNewsDto CategoryNews { get; set; }

        public virtual ViewNewsDto ViewNews { get; set; }

        public DetailNewsDto() { }

        public DetailNewsDto(DetailNews e)
        {
            Id = e.Id;
            Title = e.Title;
            Content = e.Content;
            ExpiredDate = e.ExpiredDate;
            Status = e.Status;
            CategoryNewsId = e.CategoryNewsId;

            if (e.CategoryNews != null)
            {
                CategoryNews = new CategoryNewsDto(e.CategoryNews);
            }

            if (e.ViewNews != null)
            {
                ViewNews = new ViewNewsDto(e.ViewNews);
            }
        }
    }
}
