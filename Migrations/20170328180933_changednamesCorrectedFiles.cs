using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace dojoPrep.Migrations
{
    public partial class changednamesCorrectedFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Joins_Stuff_StuffId",
                table: "Joins");

            migrationBuilder.DropIndex(
                name: "IX_Joins_StuffId",
                table: "Joins");

            migrationBuilder.DropColumn(
                name: "StuffId",
                table: "Joins");

            migrationBuilder.DropTable(
                name: "Stuff");

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    ActId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<int>(nullable: false),
                    DateEnd = table.Column<DateTime>(nullable: false),
                    DateStart = table.Column<DateTime>(nullable: false),
                    Desc = table.Column<string>(nullable: true),
                    Duration = table.Column<double>(nullable: false),
                    TimeEnd = table.Column<TimeSpan>(nullable: false),
                    TimeStart = table.Column<TimeSpan>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Units = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.ActId);
                    table.ForeignKey(
                        name: "FK_Activity_User_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<int>(
                name: "ActId",
                table: "Joins",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Joins_ActId",
                table: "Joins",
                column: "ActId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_CreatorId",
                table: "Activity",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Joins_Activity_ActId",
                table: "Joins",
                column: "ActId",
                principalTable: "Activity",
                principalColumn: "ActId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Joins_Activity_ActId",
                table: "Joins");

            migrationBuilder.DropIndex(
                name: "IX_Joins_ActId",
                table: "Joins");

            migrationBuilder.DropColumn(
                name: "ActId",
                table: "Joins");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.CreateTable(
                name: "Stuff",
                columns: table => new
                {
                    StuffId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<int>(nullable: false),
                    DateEnd = table.Column<DateTime>(nullable: false),
                    DateStart = table.Column<DateTime>(nullable: false),
                    Desc = table.Column<string>(nullable: true),
                    Duration = table.Column<double>(nullable: false),
                    TimeEnd = table.Column<TimeSpan>(nullable: false),
                    TimeStart = table.Column<TimeSpan>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stuff", x => x.StuffId);
                    table.ForeignKey(
                        name: "FK_Stuff_User_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<int>(
                name: "StuffId",
                table: "Joins",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Joins_StuffId",
                table: "Joins",
                column: "StuffId");

            migrationBuilder.CreateIndex(
                name: "IX_Stuff_CreatorId",
                table: "Stuff",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Joins_Stuff_StuffId",
                table: "Joins",
                column: "StuffId",
                principalTable: "Stuff",
                principalColumn: "StuffId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
