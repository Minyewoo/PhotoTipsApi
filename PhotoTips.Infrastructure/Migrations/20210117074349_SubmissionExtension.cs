using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoTips.Infrastructure.Migrations
{
    public partial class SubmissionExtension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Submissions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Mark",
                table: "Submissions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "Mark",
                table: "Submissions");
        }
    }
}
