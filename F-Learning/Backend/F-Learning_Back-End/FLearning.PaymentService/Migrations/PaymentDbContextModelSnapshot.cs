﻿// <auto-generated />
using System;
using FLearning.PaymentService.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FLearning.PaymentService.Migrations
{
    [DbContext(typeof(PaymentDbContext))]
    partial class PaymentDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FLearning.PaymentService.Models.Domain.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasDefaultValueSql("(newid())");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("datetime")
                        .HasColumnName("due_date");

                    b.Property<DateTime?>("InvoiceDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("invoice_date")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Status")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("status");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("student_id");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("total_amount");

                    b.HasKey("Id")
                        .HasName("PK__invoices__3213E83F56FA1DCB");

                    b.HasIndex("StudentId");

                    b.ToTable("invoices", (string)null);
                });

            modelBuilder.Entity("FLearning.PaymentService.Models.Domain.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasDefaultValueSql("(newid())");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("amount");

                    b.Property<int>("CourseId")
                        .HasColumnType("int")
                        .HasColumnName("course_id");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("PaymentMethod")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("payment_method");

                    b.Property<string>("PaymentStatus")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("payment_status");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("student_id");

                    b.Property<string>("TransactionId")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("transaction_id");

                    b.HasKey("Id")
                        .HasName("PK__payments__3213E83F54A2FA40");

                    b.HasIndex(new[] { "TransactionId" }, "UQ__payments__85C600AE7ACD37FC")
                        .IsUnique()
                        .HasFilter("[transaction_id] IS NOT NULL");

                    b.HasIndex(new[] { "StudentId" }, "UQ_payments_student")
                        .IsUnique();

                    b.ToTable("payments", (string)null);
                });

            modelBuilder.Entity("FLearning.PaymentService.Models.Domain.Refund", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasDefaultValueSql("(newid())");

                    b.Property<Guid>("PaymentId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("payment_id");

                    b.Property<decimal>("RefundAmount")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("refund_amount");

                    b.Property<string>("RefundReason")
                        .HasColumnType("text")
                        .HasColumnName("refund_reason");

                    b.Property<string>("RefundStatus")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("refund_status");

                    b.Property<DateTime?>("RefundedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("refunded_at")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("Id")
                        .HasName("PK__refunds__3213E83FAA5ED8F0");

                    b.HasIndex("PaymentId");

                    b.ToTable("refunds", (string)null);
                });

            modelBuilder.Entity("FLearning.PaymentService.Models.Domain.Invoice", b =>
                {
                    b.HasOne("FLearning.PaymentService.Models.Domain.Payment", "Student")
                        .WithMany("Invoices")
                        .HasForeignKey("StudentId")
                        .HasPrincipalKey("StudentId")
                        .IsRequired()
                        .HasConstraintName("FK__invoices__studen__46E78A0C");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("FLearning.PaymentService.Models.Domain.Refund", b =>
                {
                    b.HasOne("FLearning.PaymentService.Models.Domain.Payment", "Payment")
                        .WithMany("Refunds")
                        .HasForeignKey("PaymentId")
                        .IsRequired()
                        .HasConstraintName("FK__refunds__payment__2E1BDC42");

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("FLearning.PaymentService.Models.Domain.Payment", b =>
                {
                    b.Navigation("Invoices");

                    b.Navigation("Refunds");
                });
#pragma warning restore 612, 618
        }
    }
}
