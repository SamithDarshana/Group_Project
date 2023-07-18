using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Group_Project.Migrations
{
    /// <inheritdoc />
    public partial class paymentForDoctorAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Payment",
                table: "DoctorTable",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payment",
                table: "DoctorTable");
        }
    }
}
