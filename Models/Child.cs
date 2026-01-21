using System.ComponentModel.DataAnnotations;

namespace DaycareManager.Models;

public class Child
{
    public int Id { get; set; }
    
    [Required]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    public string LastName { get; set; } = string.Empty;
    
    public DateTime DateOfBirth { get; set; }
    
    public List<Parent> Parents { get; set; } = new();

    public int? ClassroomId { get; set; }
    public Classroom? Classroom { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";

    public string AgeDescription
    {
        get
        {
            var today = DateTime.Today;
            var dob = DateOfBirth;
            if (dob > today) return "N/A";

            int years = today.Year - dob.Year;
            int months = today.Month - dob.Month;
            int days = today.Day - dob.Day;

            if (days < 0)
            {
                months--;
                // Days in previous month
                var prevMonth = today.AddMonths(-1);
                days += DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }

            var parts = new List<string>();
            if (years > 0) parts.Add($"{years} year{(years == 1 ? "" : "s")}");
            if (months > 0) parts.Add($"{months} month{(months == 1 ? "" : "s")}");
            if (days > 0) parts.Add($"{days} day{(days == 1 ? "" : "s")}");
            
            if (parts.Count == 0) return "0 days";

            return string.Join(" ", parts);
        }
    }
}
