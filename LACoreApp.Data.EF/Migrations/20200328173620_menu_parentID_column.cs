using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LACoreApp.Data.EF.Migrations
{
    public partial class menu_parentID_column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Menus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Menus");
        }
    }
}
