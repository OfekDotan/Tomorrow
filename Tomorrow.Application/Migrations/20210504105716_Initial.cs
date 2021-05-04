using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tomorrow.Application.Migrations
{
	public partial class Initial : Migration
	{
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Todos");

			migrationBuilder.DropTable(
				name: "Groups");

			migrationBuilder.DropTable(
				name: "Accounts");
		}

		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Accounts",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Accounts", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Groups",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
					OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Groups", x => x.Id);
					table.ForeignKey(
						name: "FK_Groups_Accounts_OwnerId",
						column: x => x.OwnerId,
						principalTable: "Accounts",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Todos",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
					OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Priority = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Todos", x => x.Id);
					table.ForeignKey(
						name: "FK_Todos_Accounts_OwnerId",
						column: x => x.OwnerId,
						principalTable: "Accounts",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Todos_Groups_GroupId",
						column: x => x.GroupId,
						principalTable: "Groups",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict); //EF core will delete - ClientCascade
				});

			migrationBuilder.CreateIndex(
				name: "IX_Groups_OwnerId",
				table: "Groups",
				column: "OwnerId");

			migrationBuilder.CreateIndex(
				name: "IX_Todos_GroupId",
				table: "Todos",
				column: "GroupId");

			migrationBuilder.CreateIndex(
				name: "IX_Todos_OwnerId",
				table: "Todos",
				column: "OwnerId");
		}
	}
}