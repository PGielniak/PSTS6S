using Microsoft.EntityFrameworkCore.Migrations;

namespace PSTS6.Migrations
{
    public partial class testIdentity3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserClaims",
                table: "IdentityUserClaims");

            migrationBuilder.RenameTable(
                name: "IdentityUserClaims",
                newName: "UserClaims");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserClaims",
                table: "UserClaims",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserClaims",
                table: "UserClaims");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                newName: "IdentityUserClaims");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserClaims",
                table: "IdentityUserClaims",
                column: "Id");
        }
    }
}
