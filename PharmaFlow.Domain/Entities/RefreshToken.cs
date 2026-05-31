using System;
using System.Collections.Generic;

namespace PharmaFlow.Persistence;

public partial class RefreshToken
{
    public Guid Id { get; set; }

    public Guid UsuarioId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public string? CreatedByIp { get; set; }

    public string? ReplacedByToken { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
