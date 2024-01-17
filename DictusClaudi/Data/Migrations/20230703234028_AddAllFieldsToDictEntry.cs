using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DictusClaudi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAllFieldsToDictEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Declension",
                table: "DictEntry",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Age",
                table: "DictEntry",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "DictEntry",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Connotation",
                table: "DictEntry",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Frequency",
                table: "DictEntry",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "DictEntry",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Geo",
                table: "DictEntry",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "DictEntry",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Varient",
                table: "DictEntry",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "DictEntry");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "DictEntry");

            migrationBuilder.DropColumn(
                name: "Connotation",
                table: "DictEntry");

            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "DictEntry");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "DictEntry");

            migrationBuilder.DropColumn(
                name: "Geo",
                table: "DictEntry");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "DictEntry");

            migrationBuilder.DropColumn(
                name: "Varient",
                table: "DictEntry");

            migrationBuilder.AlterColumn<string>(
                name: "Declension",
                table: "DictEntry",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
