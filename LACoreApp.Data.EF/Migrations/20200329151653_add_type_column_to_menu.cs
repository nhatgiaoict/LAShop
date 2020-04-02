using Microsoft.EntityFrameworkCore.Migrations;

namespace LACoreApp.Data.EF.Migrations
{
    public partial class add_type_column_to_menu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HardCode",
                table: "Menus",
                newName: "Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Menus",
                newName: "HardCode");

        }
    }
}
