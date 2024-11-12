using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HSG.RestaurantTableBooking.Core;

public partial class Restaurant
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string Address { get; set; } = null!;

    [Required]
    [MaxLength(20)]
    public string? Phone { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Email { get; set; }

    [Required]
    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public virtual ICollection<RestaurantBranch> RestaurantBranches { get; set; } = new List<RestaurantBranch>();
}
