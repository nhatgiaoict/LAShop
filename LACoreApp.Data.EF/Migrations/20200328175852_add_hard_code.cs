using Microsoft.EntityFrameworkCore.Migrations;

namespace LACoreApp.Data.EF.Migrations
{
    public partial class add_hard_code : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Menus",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrdser",
                table: "Menus");
        }
    }
}
