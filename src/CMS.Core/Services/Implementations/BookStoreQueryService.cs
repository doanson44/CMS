using CMS.Core.Domains;
using CMS.Core.Services.Interfaces;
using CMS.Core.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CMS.Core.Services.Implementations
{
    public class BookStoreQueryService : IBookStoreQueryService
    {
        private readonly MultipleDatabaseSettings _connectionString;

        public BookStoreQueryService(IOptions<MultipleDatabaseSettings> connectionString)
        {
            _connectionString = connectionString.Value;
        }

        public BookSPDto GetBookStoreFromStoredProcedure(long bookId)
        {
            using var cnn = new SqlConnection(_connectionString.BookStoreDbConnectionString);
            using var cmd = new SqlCommand("dbo.GetBookDetails", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BookId", SqlDbType.BigInt) { Value = bookId });
            cnn.Open();
            using SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            var list = new List<BookSPDto>();
            while (reader.Read())
            {
                var id = long.Parse(reader[0].ToString());
                var title = reader.GetString(1);
                var categoryId = long.Parse(reader[2].ToString());
                var categoryName = reader.GetString(3);
                long? reviewId = string.IsNullOrEmpty(reader[4].ToString()) ? null : long.Parse(reader[4].ToString());
                var reviewText = reader[5].ToString();
                list.Add(new BookSPDto
                {
                    Id = id,
                    Title = title,
                    CategoryId = categoryId,
                    CategoryName = categoryName,
                    ReviewId = reviewId,
                    ReviewText = reviewText
                });
            }

            return list.FirstOrDefault();
        }
    }
}
