using Microsoft.EntityFrameworkCore.Migrations;

namespace Learn.DataLayer.Migrations
{
    public partial class language : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                schema: "dbo",
                table: "Products",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LanguageId",
                schema: "dbo",
                table: "Products");
        }
    }
}
