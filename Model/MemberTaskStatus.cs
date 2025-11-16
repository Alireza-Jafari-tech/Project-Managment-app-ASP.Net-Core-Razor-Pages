using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;

namespace AuthTest.Model
{
  // [Table("TasksStatus")]
  public class MemberTaskStatus
  {
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
    public string Name { get; set; }
    [StringLength(500, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 500 characters.")]
    public string? Description { get; set; }
    [Required]
    [Display(Name = "Css Class Name")]
    public string CssClassName { get; set; }
  }
}