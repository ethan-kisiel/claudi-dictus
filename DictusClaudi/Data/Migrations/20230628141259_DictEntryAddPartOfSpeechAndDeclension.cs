using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DictusClaudi.Data.Migrations
{
    /// <inheritdoc />
    public partial class DictEntryAddPartOfSpeechAndDeclension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Declension",
                table: "DictEntry",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartOfSpeech",
                table: "DictEntry",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Declension",
                table: "DictEntry");

            migrationBuilder.DropColumn(
                name: "PartOfSpeech",
                table: "DictEntry");
        }
    }
}
