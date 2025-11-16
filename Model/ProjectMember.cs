using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AuthTest.Model
{
  public class ProjectMember
  {    
    [Required]
    public int ProjectId { get; set; }
    
    [Required]
    public string MemberId { get; set; }
    
    [ValidateNever]
    public Project Project { get; set; }
    
    [ValidateNever]
    public ApplicationUser Member { get; set; }
    [Display(Name = "Joined At")]
    
    public DateTime JoinedAt { get; set; } = DateTime.Now;
  }
}