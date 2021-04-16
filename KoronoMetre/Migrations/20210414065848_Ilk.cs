using Microsoft.EntityFrameworkCore.Migrations;

namespace KoronoMetre.Migrations
{
    public partial class Ilk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ulkeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nufus = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ulkeler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KoronaBilgileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VakaSayisi = table.Column<int>(type: "int", nullable: false),
                    OlumSayisi = table.Column<int>(type: "int", nullable: false),
                    UlkeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoronaBilgileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KoronaBilgileri_Ulkeler_UlkeId",
                        column: x => x.UlkeId,
                        principalTable: "Ulkeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KoronaBilgileri_UlkeId",
                table: "KoronaBilgileri",
                column: "UlkeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KoronaBilgileri");

            migrationBuilder.DropTable(
                name: "Ulkeler");
        }
    }
}
