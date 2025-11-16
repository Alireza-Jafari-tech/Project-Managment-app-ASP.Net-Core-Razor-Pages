using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

using AuthTest.Data;
using AuthTest.Model;

namespace AuthTest.Pages.Member
{
  public class EditModel : PageModel
  {
    private readonly ApplicationDbContext _context;
    public EditModel(ApplicationDbContext context)
    {
      _context = context;
    }

    public class EditTask
    {
      [Display(Name = "Status")]
      public int NewStatusId { get; set; }
      [Display(Name = "Mark as Completed")]
      public bool IsCompleted { get; set; } = false;
    }

    [BindProperty]
    public EditTask EditTaskInfo { get; set; }
    public TeamMemberTask Task { get ;set; }
    public List<SelectListItem> TasksStatus { get; set; }

    
    public IActionResult OnGet(int id)
    {
      Task = _context.Tasks
      .Include(i => i.Project)
      .Include(c => c.CreatedBy)
      .Include(a => a.AssignedToMember)
      .Include(s => s.Status)
      .Include(p => p.Priority)
      .FirstOrDefault(c => c.Id == id);

      if (Task == null)
        return RedirectToPage("/NotFound");

      TasksStatus = _context.TasksStatus
      .Select(s => new SelectListItem
      {
        Value = s.Id.ToString(),
        Text = s.Name
      }).ToList();

        return Page();
    }
    public IActionResult OnPost(int id)
    {
      if (!ModelState.IsValid)
        return Page();

      var task = _context.Tasks
        .FirstOrDefault(c => c.Id == id);

      if (task == null)
        return RedirectToPage("/NotFound");

      task.StatusId = EditTaskInfo.NewStatusId;
      task.IsCompleted = EditTaskInfo.IsCompleted;

      if (EditTaskInfo.IsCompleted)
      {
        var completedStatus = _context.TasksStatus
          .FirstOrDefault(s => s.Name == "Completed" || s.Name == "Done");

        if (completedStatus != null)
          task.StatusId = completedStatus.Id;
      }
      
      _context.SaveChanges();

      return RedirectToPage("/Member/Dashboard");
    }
  }
}