using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using AuthTest.Data;
using AuthTest.Model;

namespace AuthTest.Pages
{
  public class ReadModel : PageModel
  {
    private readonly ApplicationDbContext _context;
    public TeamMemberTask Task { get; set; }

    public ReadModel(ApplicationDbContext context)
    {
      _context = context;
    }

    public void OnGet(int taskId)
    {
      Task = _context.Tasks
            .Include(i => i.Project)
            .Include(a => a.AssignedToMember)
            .Include(s => s.Status)
            .Include(p => p.Priority)
            .FirstOrDefault(c => c.Id == taskId);
    }
  }
}