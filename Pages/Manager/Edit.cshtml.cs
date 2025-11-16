using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

using AuthTest.Data;
using AuthTest.Model;

namespace AuthTest.Pages.Manager
{
  public class EditModel : PageModel
  {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    public List<SelectListItem> ProjectsDropdown { get; set; }
    public List<SelectListItem> ProjectStatusDropdown { get; set; }

    public string UserId { get; set; }
    [BindProperty(SupportsGet = true)]
    public int SelectedProjectId { get; set; } = 0;
    [BindProperty]
    public EditProject EditProjectInfo { get; set; }
    public Project Project { get; set; }

    public class EditProject
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Description { get; set; }
      [Display(Name = "Status")]
      public int StatusId { get; set; }
      [Display(Name = "Is Finished")]
      public bool IsFinished { get; set; }
    }

    public EditModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }

    public void OnGet()
    {
    UserId = _userManager.GetUserId(User);

    // Populate dropdowns
    ProjectsDropdown = _context.Projects
        .Where(p => p.CreatedById == UserId)
        .Select(p => new SelectListItem
        {
            Text = p.Name,
            Value = p.Id.ToString()
        })
        .ToList();

    ProjectStatusDropdown = _context.ProjectsStatus
        .Select(s => new SelectListItem
        {
            Text = s.Name,
            Value = s.Id.ToString()
        })
        .ToList();
    }

    public IActionResult OnPost()
    {
      if(!ModelState.IsValid)
        return Page();

      var project = _context.Projects.FirstOrDefault(c => c.Id == SelectedProjectId);

      if(project == null)
        return RedirectToPage("/NotFound");

      project.Name = EditProjectInfo.Name;
      project.Description = EditProjectInfo.Description;
      project.IsFinished = EditProjectInfo.IsFinished;
      project.StatusId = EditProjectInfo.StatusId;

      if (project.IsFinished)
      {
        var completedStatus = _context.ProjectsStatus
          .FirstOrDefault(s => s.Name == "Completed");

        if (completedStatus != null)
          project.StatusId = completedStatus.Id;
      }

      _context.SaveChanges();

      return RedirectToPage("/Manager/Dashboard");
    }
  }
}