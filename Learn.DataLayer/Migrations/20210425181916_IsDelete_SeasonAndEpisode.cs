using Microsoft.EntityFrameworkCore.Migrations;

namespace Learn.DataLayer.Migrations
{
    public partial class IsDelete_SeasonAndEpisode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "Seasons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "ProductEpisodes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "ProductEpisodes");
        }
    }
}
