using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotesAPI.Migrations
{
    /// <inheritdoc />
    public partial class firstmig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdat = table.Column<DateTime>(name: "created_at", type: "datetime2", nullable: false),
                    updatedat = table.Column<DateTime>(name: "updated_at", type: "datetime2", nullable: false),
                    chktimestamp = table.Column<byte[]>(name: "chk_timestamp", type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    NotesID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ByUserUserID = table.Column<int>(type: "int", nullable: true),
                    createdat = table.Column<DateTime>(name: "created_at", type: "datetime2", nullable: false),
                    updatedat = table.Column<DateTime>(name: "updated_at", type: "datetime2", nullable: false),
                    chktimestamp = table.Column<byte[]>(name: "chk_timestamp", type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.NotesID);
                    table.ForeignKey(
                        name: "FK_Notes_Users_ByUserUserID",
                        column: x => x.ByUserUserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ByUserUserID",
                table: "Notes",
                column: "ByUserUserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
