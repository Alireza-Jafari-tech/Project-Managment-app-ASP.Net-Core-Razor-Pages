using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AuthTest.Model
{
  public class ApplicationUser : IdentityUser
  {
    [Display(Name = "Full Name")]
    public string FullName { get; set; }
    [Display(Name = "Profile Picture URL")]
    public string ProfilePictureUrl { get; set; }
    public string Role { get; set; }
    public ICollection<TeamMemberTask>? CreatedTasks { get; set; }
    [ValidateNever]
    public ICollection<Project> CreatedProjects { get; set; }
    // public ICollection<int>? ManagedProjectsId { get; set; }
    [ValidateNever]
    public ICollection<Project> ManagedProjects { get; set; }
    public ICollection<ProjectMember>? MemberProjects { get; set; }
    // [ValidateNever]
    // public ICollection<Message>? SentMessages { get; set; }

    // [ValidateNever]
    // public ICollection<Message>? ReceivedMessages { get; set; }
    [RequiredIfRole("Manager")]
    public int? ManagerProfileId { get; set; }
    [ValidateNever]
    public ManagerProfile ManagerProfile { get; set; }

    [RequiredIfRole("TeamMember")]
    public int? MemberProfileId { get; set; }
    [ValidateNever]
    public MemberProfile MemberProfile { get; set; }
  }
}