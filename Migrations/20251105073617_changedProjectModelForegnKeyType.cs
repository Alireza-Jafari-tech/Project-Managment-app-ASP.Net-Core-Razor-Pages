using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthTest.Migrations
{
    /// <inheritdoc />
    public partial class changedProjectModelForegnKeyType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserProject");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserProject",
                columns: table => new
                {
                    MemberProjectsId = table.Column<int>(type: "int", nullable: false),
                    ProjectMembersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserProject", x => new { x.MemberProjectsId, x.ProjectMembersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserProject_AspNetUsers_ProjectMembersId",
                        column: x => x.ProjectMembersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserProject_Projects_MemberProjectsId",
                        column: x => x.MemberProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserProject_ProjectMembersId",
                table: "ApplicationUserProject",
                column: "ProjectMembersId");
        }
    }
}
