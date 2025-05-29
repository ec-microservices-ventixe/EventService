using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class PackageForm
{
    public int EventId { get; set; }

    public string Name { get; set; } = null!;

    public bool? IsSeating { get; set; }

    public string Benefits { get; set; } = null!;

    [Required]
    public float ExtraFeeInProcent { get; set; }
}
