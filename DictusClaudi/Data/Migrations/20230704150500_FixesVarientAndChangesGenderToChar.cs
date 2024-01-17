using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DictusClaudi.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixesVarientAndChangesGenderToChar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Varient",
                table: "DictEntry",
                newName: "Variant");

            migrationBuilder.AddColumn<string>(
                name: "Form",
                table: "DictEntry",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Form",
                table: "DictEntry");

            migrationBuilder.RenameColumn(
                name: "Variant",
                table: "DictEntry",
                newName: "Varient");
        }
    }
}
