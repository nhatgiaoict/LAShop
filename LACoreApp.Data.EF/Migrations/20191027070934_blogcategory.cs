using Microsoft.EntityFrameworkCore.Migrations;

namespace LACoreApp.Data.EF.Migrations
{
    public partial class blogcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_BlogCategories_BlogCategoryId",
                table: "Blogs");

            migrationBuilder.RenameColumn(
                name: "BlogCategoryId",
                table: "Blogs",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Blogs_BlogCategoryId",
                table: "Blogs",
                newName: "IX_Blogs_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_BlogCategories_CategoryId",
                table: "Blogs",
                column: "CategoryId",
                principalTable: "BlogCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_BlogCategories_CategoryId",
                table: "Blogs");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Blogs",
                newName: "BlogCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Blogs_CategoryId",
                table: "Blogs",
                newName: "IX_Blogs_BlogCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_BlogCategories_BlogCategoryId",
                table: "Blogs",
                column: "BlogCategoryId",
                principalTable: "BlogCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
