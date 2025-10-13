using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AmilaOnboarding.Server.Models;

public partial class Product
{
    public int Id { get; set; }

    [Required] public string? Name { get; set; }

    [Required] public decimal? Price { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
