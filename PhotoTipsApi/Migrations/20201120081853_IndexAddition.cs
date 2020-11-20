using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoTipsApi.Migrations
{
    public partial class IndexAddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "index_number",
                table: "modules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "index_number",
                table: "module_entries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "index_number",
                table: "lecture_contents",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "index_number",
                table: "modules");

            migrationBuilder.DropColumn(
                name: "index_number",
                table: "module_entries");

            migrationBuilder.DropColumn(
                name: "index_number",
                table: "lecture_contents");
        }
    }
}
