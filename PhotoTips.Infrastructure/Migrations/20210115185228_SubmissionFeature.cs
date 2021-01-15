using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoTips.Infrastructure.Migrations
{
    public partial class SubmissionFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    SubmitterId = table.Column<long>(type: "bigint", nullable: true),
                    ModuleEntryId = table.Column<long>(type: "bigint", nullable: true),
                    PhotoId = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_ModuleEntries_ModuleEntryId",
                        column: x => x.ModuleEntryId,
                        principalTable: "ModuleEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Submissions_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Submissions_Users_SubmitterId",
                        column: x => x.SubmitterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_ModuleEntryId",
                table: "Submissions",
                column: "ModuleEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_PhotoId",
                table: "Submissions",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_SubmitterId",
                table: "Submissions",
                column: "SubmitterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Submissions");
        }
    }
}
