using System.ComponentModel.DataAnnotations;

namespace DaycareManager.Models;

public class Classroom
{
    public int Id { get; set; }

    [Required]
    public string ClassroomNumber { get; set; } = string.Empty;

    public int MinAgeMonths { get; set; }
    public int MaxAgeMonths { get; set; }
}
