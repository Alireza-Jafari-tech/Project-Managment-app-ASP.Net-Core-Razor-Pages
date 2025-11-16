using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AuthTest.Model
{
  public class ProjectStatus
  {
    [Key]
    public int Id { get; set; }
    [Required]
    [Display(Name = "Status")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Priority name must be between 3 and 50 characters.")]
    public string Name { get; set; }
    [Display(Name = "Description")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 200 characters.")]
    public string? Description { get; set; }
    [Required]
    [Display(Name = "Css Class Name")]
    public string CssClassName { get; set; }
  }
}