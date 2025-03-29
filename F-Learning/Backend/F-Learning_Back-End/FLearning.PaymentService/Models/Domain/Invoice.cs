using System;
using System.Collections.Generic;

namespace FLearning.PaymentService.Models.Domain;

public partial class Invoice
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public DateTime? DueDate { get; set; }

    public string? Status { get; set; }

    public virtual Payment Student { get; set; } = null!;
}
