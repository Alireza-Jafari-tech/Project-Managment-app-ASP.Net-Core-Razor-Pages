using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AuthTest.Model
{
  public class MemberExperienceLevel
  {
    [Key]
    public int Id { get; set; }
    [StringLength(500, MinimumLength = 3, ErrorMessage = "Level Description must be between 3 and 500 characters.")]
    public string? Description { get; set; }
    [Required]
    [Display(Name = "Lavel Name")]
    public string LevelName { get; set; }
  }
}