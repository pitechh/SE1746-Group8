using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FLearning.PaymentService.Models.Domain;

public partial class PaymentDbContext : DbContext
{
    public PaymentDbContext()
    {
    }

    public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Refund> Refunds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfigurationRoot configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("PaymentConnectionString"));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__invoices__3213E83F56FA1DCB");

            entity.ToTable("invoices");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.DueDate)
                .HasColumnType("datetime")
                .HasColumnName("due_date");
            entity.Property(e => e.InvoiceDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("invoice_date");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_amount");

            entity.HasOne(d => d.Student).WithMany(p => p.Invoices)
                .HasPrincipalKey(p => p.StudentId)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__invoices__studen__46E78A0C");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__payments__3213E83F54A2FA40");

            entity.ToTable("payments");

            entity.HasIndex(e => e.TransactionId, "UQ__payments__85C600AE7ACD37FC").IsUnique();

            entity.HasIndex(e => e.StudentId, "UQ_payments_student").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("payment_method");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("payment_status");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("transaction_id");
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__refunds__3213E83FAA5ED8F0");

            entity.ToTable("refunds");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.RefundAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("refund_amount");
            entity.Property(e => e.RefundReason)
                .HasColumnType("text")
                .HasColumnName("refund_reason");
            entity.Property(e => e.RefundStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("refund_status");
            entity.Property(e => e.RefundedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("refunded_at");

            entity.HasOne(d => d.Payment).WithMany(p => p.Refunds)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__refunds__payment__2E1BDC42");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
