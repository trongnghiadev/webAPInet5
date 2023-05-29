using Microsoft.EntityFrameworkCore.Migrations;

namespace HocHanh6day.Migrations
{
    public partial class DBV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaLoai",
                table: "HangHoas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Loai",
                columns: table => new
                {
                    MaLoai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoai = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loai", x => x.MaLoai);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HangHoas_MaLoai",
                table: "HangHoas",
                column: "MaLoai");

            migrationBuilder.AddForeignKey(
                name: "FK_HangHoas_Loai_MaLoai",
                table: "HangHoas",
                column: "MaLoai",
                principalTable: "Loai",
                principalColumn: "MaLoai",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HangHoas_Loai_MaLoai",
                table: "HangHoas");

            migrationBuilder.DropTable(
                name: "Loai");

            migrationBuilder.DropIndex(
                name: "IX_HangHoas_MaLoai",
                table: "HangHoas");

            migrationBuilder.DropColumn(
                name: "MaLoai",
                table: "HangHoas");
        }
    }
}
