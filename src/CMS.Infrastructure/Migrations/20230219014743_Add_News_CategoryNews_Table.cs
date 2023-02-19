using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace CMS.Infrastructure.Migrations
{
    public partial class Add_News_CategoryNews_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_User_AuthorId",
                table: "TodoItems");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_AuthorId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "TodoItems");

            migrationBuilder.CreateTable(
                name: "CategoryNews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryNews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DetailNews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(250)", maxLength: 250, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReferenceBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryNewsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailNews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailNews_CategoryNews_CategoryNewsId",
                        column: x => x.CategoryNewsId,
                        principalTable: "CategoryNews",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ViewNews",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<int>(type: "int", nullable: false),
                    DetailNewsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewNews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViewNews_DetailNews_DetailNewsId",
                        column: x => x.DetailNewsId,
                        principalTable: "DetailNews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetailNews_CategoryNewsId",
                table: "DetailNews",
                column: "CategoryNewsId");

            migrationBuilder.CreateIndex(
                name: "IX_ViewNews_DetailNewsId",
                table: "ViewNews",
                column: "DetailNewsId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViewNews");

            migrationBuilder.DropTable(
                name: "DetailNews");

            migrationBuilder.DropTable(
                name: "CategoryNews");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "TodoItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_AuthorId",
                table: "TodoItems",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_User_AuthorId",
                table: "TodoItems",
                column: "AuthorId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
