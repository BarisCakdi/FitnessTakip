using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTakip.Migrations
{
    /// <inheritdoc />
    public partial class DeleteLessons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPrograms_Lessons_LessonId",
                table: "UserPrograms");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_UserPrograms_LessonId",
                table: "UserPrograms");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "UserPrograms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "UserPrograms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUserId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                });

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
    }
}
