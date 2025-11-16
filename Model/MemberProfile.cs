using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AuthTest.Model
{
  public class MemberProfile
  {
    [Key]
    public int Id { get; set; }
    [Required]
    [Display(Name = "Job Title")]
    public string JobTitle { get; set; }
    [Required]
    [Display(Name = "Skill Set")]
    public string SkillSet { get; set; }
    public int MemberExperienceLevelId { get; set; }
    [ValidateNever]
    [Display(Name = "Member Experience Level")]
    public MemberExperienceLevel MemberExperienceLevel { get; set; }
    public List<TeamMemberTask> Tasks { get; set; } = new List<TeamMemberTask>();
  }
}