using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AuthTest.Model
{
  public class TaskFilter
  {
    [Display(Name = "Select Status")]
    public int? SelectedStatusId { get; set; }
    [Display(Name = "Select Priority")]
    public int? SelectedPriorityId { get; set; }
    [Display(Name = "Select Team member")]
    public string? SelectedMemberId { get; set; }
    [Display(Name = "Select Project")]
    public int? SelectedProjectId { get; set; }
  }
}