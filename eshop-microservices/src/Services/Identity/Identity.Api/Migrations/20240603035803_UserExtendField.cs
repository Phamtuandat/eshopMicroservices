using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Api.Migrations
{
    /// <inheritdoc />
    public partial class UserExtendField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressLine",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Birthday",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "AspNetUsers");
        }
    }
}
