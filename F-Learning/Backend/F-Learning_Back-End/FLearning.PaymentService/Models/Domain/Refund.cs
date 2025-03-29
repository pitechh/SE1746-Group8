using System;
using System.Collections.Generic;

namespace FLearning.PaymentService.Models.Domain;

public partial class Refund
{
    public Guid Id { get; set; }

    public Guid PaymentId { get; set; }

    public decimal RefundAmount { get; set; }

    public string? RefundReason { get; set; }

    public string? RefundStatus { get; set; }

    public DateTime? RefundedAt { get; set; }

    public virtual Payment Payment { get; set; } = null!;
}
