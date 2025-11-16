using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AuthTest.Model
{
  public class Project
  {
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Project name must be between 3 and 200 characters.")]
    public string Name { get; set; }
    [StringLength(1000, MinimumLength = 3, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; set; }
    public string? CreatedById { get; set; }
    [Display(Name = "Created By")]
    [ValidateNever]
    public ApplicationUser CreatedBy { get; set; }
    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Display(Name = "Is Finished")]
    public bool IsFinished { get; set; } = false;
    public string? ManagerId { get; set; }
    public ApplicationUser Manager { get; set; }
    public int StatusId { get; set; }
    [ValidateNever]
    public ProjectStatus Status { get; set; }
    [ValidateNever]
    public ICollection<ProjectMember> ProjectMembers { get; set; }
  }
}