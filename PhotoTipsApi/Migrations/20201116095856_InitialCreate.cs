using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoTipsApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cities", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "modules",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_modules", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    is_admin = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    surname = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    registration_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    residence_city_id = table.Column<string>(type: "text", nullable: true),
                    password_hash = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_cities_residence_city_id",
                        column: x => x.residence_city_id,
                        principalTable: "cities",
                        principalColumn: "name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "module_entries",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    module_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_module_entries", x => x.id);
                    table.ForeignKey(
                        name: "FK_module_entries_modules_module_id",
                        column: x => x.module_id,
                        principalTable: "modules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    file_url = table.Column<string>(type: "text", nullable: true),
                    thumbnail_url = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.id);
                    table.ForeignKey(
                        name: "FK_Photos_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "lecture_contents",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "text", nullable: true),
                    ModuleEntryId = table.Column<string>(type: "text", nullable: true),
                    ModuleEntryId1 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lecture_contents", x => x.id);
                    table.ForeignKey(
                        name: "FK_lecture_contents_module_entries_ModuleEntryId",
                        column: x => x.ModuleEntryId,
                        principalTable: "module_entries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_lecture_contents_module_entries_ModuleEntryId1",
                        column: x => x.ModuleEntryId1,
                        principalTable: "module_entries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_lecture_contents_ModuleEntryId",
                table: "lecture_contents",
                column: "ModuleEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_lecture_contents_ModuleEntryId1",
                table: "lecture_contents",
                column: "ModuleEntryId1");

            migrationBuilder.CreateIndex(
                name: "IX_module_entries_module_id",
                table: "module_entries",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_UserId",
                table: "Photos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_residence_city_id",
                table: "users",
                column: "residence_city_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lecture_contents");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "module_entries");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "modules");

            migrationBuilder.DropTable(
                name: "cities");
        }
    }
}
