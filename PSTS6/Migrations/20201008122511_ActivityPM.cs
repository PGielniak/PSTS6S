using Microsoft.EntityFrameworkCore.Migrations;

namespace PSTS6.Migrations
{
    public partial class ActivityPM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Activity",
                nullable: true);

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Activity");

           
        }
    }
}
