using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotesAPI.Migrations
{
    /// <inheritdoc />
    public partial class ThirdMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Users_ByUserUserID",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_ByUserUserID",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "ByUserUserID",
                table: "Notes");

            migrationBuilder.AddColumn<string>(
               name: "ByUserId",
               table: "Notes",
               type: "nvarchar(450)",
               nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ByUserId",
                table: "Notes",
                column: "ByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_AspNetUsers_ByUserId",
                table: "Notes",
                column: "ByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_AspNetUsers_ByUserId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_ByUserId",
                table: "Notes");

            migrationBuilder.DropColumn(
              name: "ByUserId",
              table: "Notes");

            migrationBuilder.AddColumn<int>(
               name: "ByUserUserID",
               table: "Notes",
               type: "int",
               nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ByUserUserID",
                table: "Notes",
                column: "ByUserUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Users_ByUserUserID",
                table: "Notes",
                column: "ByUserUserID",
                principalTable: "Users",
                principalColumn: "UserID");
        }
    }
}
