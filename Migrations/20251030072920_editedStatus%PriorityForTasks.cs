using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthTest.Migrations
{
    /// <inheritdoc />
    public partial class editedStatusPriorityForTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemberExperienceLevelId",
                table: "MemberProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MemberProfiles_MemberExperienceLevelId",
                table: "MemberProfiles",
                column: "MemberExperienceLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberProfiles_MemberExperienceLevels_MemberExperienceLevelId",
                table: "MemberProfiles",
                column: "MemberExperienceLevelId",
                principalTable: "MemberExperienceLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberProfiles_MemberExperienceLevels_MemberExperienceLevelId",
                table: "MemberProfiles");

            migrationBuilder.DropIndex(
                name: "IX_MemberProfiles_MemberExperienceLevelId",
                table: "MemberProfiles");

            migrationBuilder.DropColumn(
                name: "MemberExperienceLevelId",
                table: "MemberProfiles");
        }
    }
}
