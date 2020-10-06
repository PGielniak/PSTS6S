using Microsoft.EntityFrameworkCore.Migrations;

namespace PSTS6.Migrations
{
    public partial class dunno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityTemplate_TaskTemplate_TaskTemplateID",
                table: "ActivityTemplate");

            migrationBuilder.AlterColumn<int>(
                name: "TaskTemplateID",
                table: "ActivityTemplate",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityTemplate_TaskTemplate_TaskTemplateID",
                table: "ActivityTemplate",
                column: "TaskTemplateID",
                principalTable: "TaskTemplate",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityTemplate_TaskTemplate_TaskTemplateID",
                table: "ActivityTemplate");

            migrationBuilder.AlterColumn<int>(
                name: "TaskTemplateID",
                table: "ActivityTemplate",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityTemplate_TaskTemplate_TaskTemplateID",
                table: "ActivityTemplate",
                column: "TaskTemplateID",
                principalTable: "TaskTemplate",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
