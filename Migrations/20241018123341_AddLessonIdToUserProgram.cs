using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTakip.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonIdToUserProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Histories");

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "UserPrograms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserPrograms_LessonId",
                table: "UserPrograms",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPrograms_Lessons_LessonId",
                table: "UserPrograms",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPrograms_Lessons_LessonId",
                table: "UserPrograms");

            migrationBuilder.DropIndex(
                name: "IX_UserPrograms_LessonId",
                table: "UserPrograms");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "UserPrograms");

            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntryDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserProgramId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.Id);
                });
        }
    }
}
