namespace CMS.Core.Domains;

public class BookSPDto
{
    public long Id { get; set; }

    public string Title { get; set; }

    public long CategoryId { get; set; }

    public string CategoryName { get; set; }

    public long? ReviewId { get; set; }

    public string ReviewText { get; set; }
}
