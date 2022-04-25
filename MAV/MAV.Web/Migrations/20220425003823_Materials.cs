using Microsoft.EntityFrameworkCore.Migrations;

namespace MAV.Web.Migrations
{
    public partial class Materials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Materials",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Materials");
        }
    }
}
