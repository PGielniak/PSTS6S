using Microsoft.EntityFrameworkCore.Migrations;

namespace PSTS6.Migrations
{
    public partial class PMtoString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_User_ProjectManagerId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_ProjectManagerId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProjectManagerId",
                table: "Project");

            migrationBuilder.AddColumn<string>(
                name: "ProjectManager",
                table: "Project",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectManager",
                table: "Project");

            migrationBuilder.AddColumn<string>(
                name: "ProjectManagerId",
                table: "Project",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_ProjectManagerId",
                table: "Project",
                column: "ProjectManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_User_ProjectManagerId",
                table: "Project",
                column: "ProjectManagerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
