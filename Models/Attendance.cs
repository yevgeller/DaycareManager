using System.ComponentModel.DataAnnotations;

namespace DaycareManager.Models;

public class Attendance
{
    public int Id { get; set; }
    
    public int ChildId { get; set; }
    public Child? Child { get; set; }
    
    public DateTime CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
}
