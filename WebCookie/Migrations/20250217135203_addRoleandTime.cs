using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCookie.Migrations
{
    /// <inheritdoc />
    public partial class addRoleandTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UltimaConexion",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UltimaConexion",
                table: "AspNetUsers");
        }
    }
}
