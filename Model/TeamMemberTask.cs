using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AuthTest.Model
{
  public class TeamMemberTask
  {
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(400, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 400 characters.")]
    public string Title { get; set; }
    [StringLength(1000, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 400 characters.")]
    public string? Description { get; set; }
    [Required]
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Required]
    public string CreatedById { get; set; }
    [ValidateNever]
    [Display(Name = "Created By")]
    public ApplicationUser CreatedBy { get; set; }
    public string AssignedToMemberId { get; set; }
    [ValidateNever]
    [Display(Name = "Assigned To")]
    public ApplicationUser AssignedToMember { get; set; }
    [Required]
    [Display(Name = "Due Date")]
    public DateTime DueDate { get; set; }
    [Display(Name = "Is Completed")]
    public bool IsCompleted { get; set; } = false;
    [Required]
    public int StatusId { get; set; }
    [ValidateNever]
    public MemberTaskStatus Status { get; set; }

    public int PriorityId { get; set; }
    [ValidateNever]
    public TaskPriority Priority { get; set; }
  }
}    