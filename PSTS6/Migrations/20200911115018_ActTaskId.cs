using Microsoft.EntityFrameworkCore.Migrations;

namespace PSTS6.Migrations
{
    public partial class ActTaskId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Task_TaskID",
                table: "Activity");

            migrationBuilder.AlterColumn<int>(
                name: "TaskID",
                table: "Activity",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Task_TaskID",
                table: "Activity",
                column: "TaskID",
                principalTable: "Task",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Task_TaskID",
                table: "Activity");

            migrationBuilder.AlterColumn<int>(
                name: "TaskID",
                table: "Activity",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Task_TaskID",
                table: "Activity",
                column: "TaskID",
                principalTable: "Task",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
