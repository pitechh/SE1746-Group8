using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FLearning.PaymentService.Migrations
{
    /// <inheritdoc />
    public partial class Update_CourseId_ToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    student_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    course_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    payment_status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    payment_method = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    transaction_id = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__payments__3213E83F54A2FA40", x => x.id);
                    table.UniqueConstraint("AK_payments_student_id", x => x.student_id);
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    student_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    invoice_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    due_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__invoices__3213E83F56FA1DCB", x => x.id);
                    table.ForeignKey(
                        name: "FK__invoices__studen__46E78A0C",
                        column: x => x.student_id,
                        principalTable: "payments",
                        principalColumn: "student_id");
                });

            migrationBuilder.CreateTable(
                name: "refunds",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    payment_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    refund_amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    refund_reason = table.Column<string>(type: "text", nullable: true),
                    refund_status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    refunded_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__refunds__3213E83FAA5ED8F0", x => x.id);
                    table.ForeignKey(
                        name: "FK__refunds__payment__2E1BDC42",
                        column: x => x.payment_id,
                        principalTable: "payments",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_invoices_student_id",
                table: "invoices",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "UQ__payments__85C600AE7ACD37FC",
                table: "payments",
                column: "transaction_id",
                unique: true,
                filter: "[transaction_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_payments_student",
                table: "payments",
                column: "student_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_refunds_payment_id",
                table: "refunds",
                column: "payment_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "refunds");

            migrationBuilder.DropTable(
                name: "payments");
        }
    }
}
