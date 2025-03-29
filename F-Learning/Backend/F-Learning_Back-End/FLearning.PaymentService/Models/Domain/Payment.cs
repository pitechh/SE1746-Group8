using System;
using System.Collections.Generic;

namespace FLearning.PaymentService.Models.Domain;

public partial class Payment
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public int CourseId { get; set; }

    public decimal Amount { get; set; }

    public string? PaymentStatus { get; set; }

    public string? PaymentMethod { get; set; }

    public string? TransactionId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();
}
