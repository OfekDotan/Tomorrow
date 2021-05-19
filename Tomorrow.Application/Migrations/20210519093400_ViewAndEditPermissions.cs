using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tomorrow.Application.Migrations
{
    public partial class ViewAndEditPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TodoEditPermissions",
                columns: table => new
                {
                    EditableTodosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    accountsThatCanEditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoEditPermissions", x => new { x.EditableTodosId, x.accountsThatCanEditId });
                    table.ForeignKey(
                        name: "FK_TodoEditPermissions_Accounts_accountsThatCanEditId",
                        column: x => x.accountsThatCanEditId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TodoEditPermissions_Todos_EditableTodosId",
                        column: x => x.EditableTodosId,
                        principalTable: "Todos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TodoViewPermissions",
                columns: table => new
                {
                    ViewableTodosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    accountsThatCanViewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoViewPermissions", x => new { x.ViewableTodosId, x.accountsThatCanViewId });
                    table.ForeignKey(
                        name: "FK_TodoViewPermissions_Accounts_accountsThatCanViewId",
                        column: x => x.accountsThatCanViewId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TodoViewPermissions_Todos_ViewableTodosId",
                        column: x => x.ViewableTodosId,
                        principalTable: "Todos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoEditPermissions_accountsThatCanEditId",
                table: "TodoEditPermissions",
                column: "accountsThatCanEditId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoViewPermissions_accountsThatCanViewId",
                table: "TodoViewPermissions",
                column: "accountsThatCanViewId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoEditPermissions");

            migrationBuilder.DropTable(
                name: "TodoViewPermissions");
        }
    }
}
