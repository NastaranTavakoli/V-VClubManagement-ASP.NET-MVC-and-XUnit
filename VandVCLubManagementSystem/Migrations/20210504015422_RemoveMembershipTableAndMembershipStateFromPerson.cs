using Microsoft.EntityFrameworkCore.Migrations;

namespace VandVCLubManagementSystem.Migrations
{
    public partial class RemoveMembershipTableAndMembershipStateFromPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_MembershipStates_MembershipStateId",
                table: "People");

            migrationBuilder.DropTable(
                name: "MembershipStates");

            migrationBuilder.DropIndex(
                name: "IX_People_MembershipStateId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "MembershipStateId",
                table: "People");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MembershipStateId",
                table: "People",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MembershipStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipStates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_People_MembershipStateId",
                table: "People",
                column: "MembershipStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_People_MembershipStates_MembershipStateId",
                table: "People",
                column: "MembershipStateId",
                principalTable: "MembershipStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
