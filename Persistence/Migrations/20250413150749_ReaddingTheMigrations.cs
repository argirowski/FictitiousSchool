using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReaddingTheMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseDates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseDates_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubmitApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    CourseDateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmitApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubmitApplications_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubmitApplications_CourseDates_CourseDateId",
                        column: x => x.CourseDateId,
                        principalTable: "CourseDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubmitApplications_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmitApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participants_SubmitApplications_SubmitApplicationId",
                        column: x => x.SubmitApplicationId,
                        principalTable: "SubmitApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Cutting trees, the ins and outs" },
                    { 2, "CSS and you - a love story" },
                    { 3, "Baking mud cakes using actual mud" },
                    { 4, "Christmas eve - myth or reality?" },
                    { 5, "LEGO colors through time" }
                });

            migrationBuilder.InsertData(
                table: "CourseDates",
                columns: new[] { "Id", "CourseId", "Date" },
                values: new object[,]
                {
                    { new Guid("2b7d2ae2-0214-42a0-be07-f6f7bcdb8064"), 3, new DateTime(2018, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("361b541c-c1af-46c6-b28f-7576d87bd51c"), 4, new DateTime(2017, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("48e2f6e5-e7a5-4861-9563-4823774a6b5c"), 4, new DateTime(2018, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("49ed75aa-0a08-40c8-92ed-cd88a68f564d"), 1, new DateTime(2017, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("587cdc3e-40a8-43f1-b67e-251292d94f3e"), 2, new DateTime(2017, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("5cd91938-abc4-440f-b3c1-52371516bf8d"), 1, new DateTime(2017, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("7cbf70c1-5cd9-435d-b39d-cd6bcd3bd7ee"), 3, new DateTime(2019, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("84dc067d-b953-42a2-afd2-fbbf4ef781f8"), 5, new DateTime(2017, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("8de1a562-ca5c-4dd3-9961-4b03650a6e47"), 3, new DateTime(2017, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("8f516d6a-e1b3-4d66-b0d7-f9b40cdcdb04"), 2, new DateTime(2017, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("c694a722-4015-45ed-8f79-628b6050e664"), 4, new DateTime(2019, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("ea3cf94a-7ab6-4347-a0a2-b8f32d2ba51b"), 3, new DateTime(2017, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0ca2d3e-554f-459e-9045-dce2d5ab616b"), 2, new DateTime(2017, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseDates_CourseId",
                table: "CourseDates",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_SubmitApplicationId",
                table: "Participants",
                column: "SubmitApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmitApplications_CompanyId",
                table: "SubmitApplications",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmitApplications_CourseDateId",
                table: "SubmitApplications",
                column: "CourseDateId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmitApplications_CourseId",
                table: "SubmitApplications",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "SubmitApplications");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "CourseDates");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
