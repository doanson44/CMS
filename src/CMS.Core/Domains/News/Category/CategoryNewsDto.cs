using CMS.Core.Data.Entities;

namespace CMS.Core.Domains;

public class CategoryNewsDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public CategoryNewsDto(CategoryNews e)
    {
        Id = e.Id;
        Name = e.Name;
    }
}
