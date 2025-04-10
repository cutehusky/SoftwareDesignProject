using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftwareDesignProject.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_starred_plugins");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Plugins",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserPlugins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PluginId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlugins", x => new { x.UserId, x.PluginId });
                    table.ForeignKey(
                        name: "FK_UserPlugins_Plugins_PluginId",
                        column: x => x.PluginId,
                        principalTable: "Plugins",
                        principalColumn: "PluginId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPlugins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plugins_UserId",
                table: "Plugins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlugins_PluginId",
                table: "UserPlugins",
                column: "PluginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plugins_Users_UserId",
                table: "Plugins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plugins_Users_UserId",
                table: "Plugins");

            migrationBuilder.DropTable(
                name: "UserPlugins");

            migrationBuilder.DropIndex(
                name: "IX_Plugins_UserId",
                table: "Plugins");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Plugins");

            migrationBuilder.CreateTable(
                name: "user_starred_plugins",
                columns: table => new
                {
                    StarredPluginId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_starred_plugins", x => new { x.StarredPluginId, x.UserId });
                    table.ForeignKey(
                        name: "FK_user_starred_plugins_Plugins_StarredPluginId",
                        column: x => x.StarredPluginId,
                        principalTable: "Plugins",
                        principalColumn: "PluginId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_starred_plugins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_starred_plugins_UserId",
                table: "user_starred_plugins",
                column: "UserId");
        }
    }
}
