using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adventure.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSpellSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequiresAttackRoll",
                table: "Spells",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SpellSlotsJson",
                table: "Characters",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterKnownSpells_SpellId",
                table: "CharacterKnownSpells",
                column: "SpellId");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterKnownSpells_Characters_CharacterId",
                table: "CharacterKnownSpells",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterKnownSpells_Spells_SpellId",
                table: "CharacterKnownSpells",
                column: "SpellId",
                principalTable: "Spells",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterKnownSpells_Characters_CharacterId",
                table: "CharacterKnownSpells");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterKnownSpells_Spells_SpellId",
                table: "CharacterKnownSpells");

            migrationBuilder.DropIndex(
                name: "IX_CharacterKnownSpells_SpellId",
                table: "CharacterKnownSpells");

            migrationBuilder.DropColumn(
                name: "RequiresAttackRoll",
                table: "Spells");

            migrationBuilder.DropColumn(
                name: "SpellSlotsJson",
                table: "Characters");
        }
    }
}
