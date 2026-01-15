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
}
