using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AmilaOnboarding.Server.Models;

public partial class Store
{
    public int Id { get; set; }

    [Required] public string? Name { get; set; }

    [Required, MaxLength(200)] public string? Address { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
