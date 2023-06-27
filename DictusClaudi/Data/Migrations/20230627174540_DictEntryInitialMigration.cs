using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DictusClaudi.Data.Migrations
{
    /// <inheritdoc />
    public partial class DictEntryInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DictEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WordStem = table.Column<string>(type: "TEXT", nullable: true),
                    WordTranslation = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictEntry", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DictEntry");
        }
    }
}
