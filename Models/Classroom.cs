using System.ComponentModel.DataAnnotations;

namespace DaycareManager.Models;

public class Classroom
{
    public int Id { get; set; }

    [Required]
    public string ClassroomNumber { get; set; } = string.Empty;

    public int MinAgeMonths { get; set; }
    public int MaxAgeMonths { get; set; }

    [Range(1, 100)]
    public int Capacity { get; set; }
}
