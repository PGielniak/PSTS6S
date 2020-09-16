using Microsoft.EntityFrameworkCore.Migrations;

namespace PSTS6.Migrations
{
    public partial class Templates3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTemplate_ProjectTemplate_ProjectTemplateID",
                table: "TaskTemplate");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectTemplateID",
                table: "TaskTemplate",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTemplate_ProjectTemplate_ProjectTemplateID",
                table: "TaskTemplate",
                column: "ProjectTemplateID",
                principalTable: "ProjectTemplate",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTemplate_ProjectTemplate_ProjectTemplateID",
                table: "TaskTemplate");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectTemplateID",
                table: "TaskTemplate",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTemplate_ProjectTemplate_ProjectTemplateID",
                table: "TaskTemplate",
                column: "ProjectTemplateID",
                principalTable: "ProjectTemplate",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
