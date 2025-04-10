using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftwareDesignProject.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStarredInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plugins_Users_UserId",
                table: "Plugins");

            migrationBuilder.DropIndex(
                name: "IX_Plugins_UserId",
                table: "Plugins");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Plugins");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Plugins",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plugins_UserId",
                table: "Plugins",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plugins_Users_UserId",
                table: "Plugins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
