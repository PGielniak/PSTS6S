using Microsoft.EntityFrameworkCore.Migrations;

namespace PSTS6.Migrations
{
    public partial class Templates2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectTemplateID",
                table: "TaskTemplate",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskTemplateID",
                table: "ActivityTemplate",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskTemplate_ProjectTemplateID",
                table: "TaskTemplate",
                column: "ProjectTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTemplate_TaskTemplateID",
                table: "ActivityTemplate",
                column: "TaskTemplateID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityTemplate_TaskTemplate_TaskTemplateID",
                table: "ActivityTemplate",
                column: "TaskTemplateID",
                principalTable: "TaskTemplate",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTemplate_ProjectTemplate_ProjectTemplateID",
                table: "TaskTemplate",
                column: "ProjectTemplateID",
                principalTable: "ProjectTemplate",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityTemplate_TaskTemplate_TaskTemplateID",
                table: "ActivityTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskTemplate_ProjectTemplate_ProjectTemplateID",
                table: "TaskTemplate");

            migrationBuilder.DropIndex(
                name: "IX_TaskTemplate_ProjectTemplateID",
                table: "TaskTemplate");

            migrationBuilder.DropIndex(
                name: "IX_ActivityTemplate_TaskTemplateID",
                table: "ActivityTemplate");

            migrationBuilder.DropColumn(
                name: "ProjectTemplateID",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TaskTemplateID",
                table: "ActivityTemplate");
        }
    }
}
