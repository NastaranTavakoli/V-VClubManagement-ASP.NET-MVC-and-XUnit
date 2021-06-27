using Microsoft.EntityFrameworkCore.Migrations;

namespace VandVCLubManagementSystem.Migrations
{
    public partial class PopulateMembershipStatesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO MembershipStates VALUES ('Active')");
            migrationBuilder.Sql("INSERT INTO MembershipStates VALUES ('Cancelled')");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM MembershipStates");
        }
    }
}
