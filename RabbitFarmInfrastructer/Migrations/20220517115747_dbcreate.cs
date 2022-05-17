using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RabbitFarmInfrastructer.Migrations
{
    public partial class dbcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_Animals",
                columns: table => new
                {
                    AnimalId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AnimalSexType = table.Column<int>(type: "INTEGER", nullable: false),
                    AnimalType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    AnimalStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    InseminationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PregnantCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedUser = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRemoved = table.Column<bool>(type: "INTEGER", nullable: false),
                    UpdatetdDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatetdUser = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Animals", x => x.AnimalId);
                });

            migrationBuilder.CreateTable(
                name: "T_AppDimensions",
                columns: table => new
                {
                    DimensionId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AnimalType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    MaxDoPerBornCount = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxDoBornCount = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxLifeMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    Pregnancyduration = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    AppStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    AppLifeTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    WaitMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedUser = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRemoved = table.Column<bool>(type: "INTEGER", nullable: false),
                    UpdatetdDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatetdUser = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_AppDimensions", x => x.DimensionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_Animals");

            migrationBuilder.DropTable(
                name: "T_AppDimensions");
        }
    }
}
