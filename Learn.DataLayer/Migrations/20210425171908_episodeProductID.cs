using Microsoft.EntityFrameworkCore.Migrations;

namespace Learn.DataLayer.Migrations
{
    public partial class episodeProductID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "dbo",
                table: "ProductEpisodes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                schema: "dbo",
                table: "ProductEpisodes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
