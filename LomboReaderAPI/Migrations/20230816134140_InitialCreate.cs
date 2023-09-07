using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LimboReaderAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "LimboReaderDB");

            
            migrationBuilder.CreateTable(
                name: "Authors",
                schema: "LimboReaderDB",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecondName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Comments",
                schema: "LimboReaderDB",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BookArticle_Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    User = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Comment = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Genres",
                schema: "LimboReaderDB",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GenreName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SubGenres",
                schema: "LimboReaderDB",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Genre_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SubGenreName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubGenres", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "LimboReaderDB",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Login = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserRole = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Avatar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegisterDt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastLoginDt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ActivateCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BookArticles",
                schema: "LimboReaderDB",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    User_idId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TitleImgPath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Author_idId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Genre_idId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SubGenre_idId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Rating = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookArticles", x => x.Id);
                   
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                schema: "LimboReaderDB",
                table: "Users",
                columns: new[] { "Id", "ActivateCode", "Active", "Avatar", "Email", "LastLoginDt", "Login", "Name", "PasswordHash", "RegisterDt", "UserRole" },
                values: new object[] { new Guid("6764db38-5306-4e85-a15d-7392e8422b8a"), null, false, null, "admin@ukr.net", null, "Admin", "Root Administrator", "CDE2E51D593001C6392593A3332BCEE452B61273", new DateTime(2023, 8, 16, 16, 41, 40, 686, DateTimeKind.Local).AddTicks(4345), "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_BookArticles_Author_idId",
                schema: "LimboReaderDB",
                table: "BookArticles",
                column: "Author_idId");

            migrationBuilder.CreateIndex(
                name: "IX_BookArticles_Genre_idId",
                schema: "LimboReaderDB",
                table: "BookArticles",
                column: "Genre_idId");

            migrationBuilder.CreateIndex(
                name: "IX_BookArticles_SubGenre_idId",
                schema: "LimboReaderDB",
                table: "BookArticles",
                column: "SubGenre_idId");

            migrationBuilder.CreateIndex(
                name: "IX_BookArticles_User_idId",
                schema: "LimboReaderDB",
                table: "BookArticles",
                column: "User_idId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookArticles",
                schema: "LimboReaderDB");

            migrationBuilder.DropTable(
                name: "Comments",
                schema: "LimboReaderDB");

            migrationBuilder.DropTable(
                name: "Authors",
                schema: "LimboReaderDB");

            migrationBuilder.DropTable(
                name: "Genres",
                schema: "LimboReaderDB");

            migrationBuilder.DropTable(
                name: "SubGenres",
                schema: "LimboReaderDB");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "LimboReaderDB");
        }
    }
}
