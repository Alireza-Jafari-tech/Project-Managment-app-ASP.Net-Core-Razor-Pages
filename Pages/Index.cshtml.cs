using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AuthTest.Model;
using AuthTest.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AuthTest.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public IQueryable<TeamMemberTask> TasksQuery { get; set; }
        public List<TeamMemberTask> TasksList { get; set; }
        public List<SelectListItem> Projects { get; set; }
        [BindProperty]
        public int SelectedProjectId { get; set; }

        public IndexModel(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
            _context = context;
            _userManager = userManager;
    }
        public void OnGet()
        {
            var userId = _userManager.GetUserId(User);

            if(User.IsInRole("Manager"))
            {
                Projects = _context.Projects
                .Where(c => c.CreatedById == userId)
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString()
                })
                .ToList();
            }
            else if (User.IsInRole("TeamMember"))
            {
                Projects = _context.ProjectMembers
                .Where(c => c.MemberId == userId)
                .Select(s => new SelectListItem
                {
                    Text = s.Project.Name,
                    Value = s.ProjectId.ToString()
                })
                .ToList();
            }
        }

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {

            }

            TasksQuery = _context.Tasks
                    .Include(i => i.AssignedToMember)
                    .Include(s => s.Status)
                    .Include(p => p.Priority)
                    .Where(c => c.ProjectId == SelectedProjectId)
                    .AsQueryable();

            TasksList = TasksQuery.ToList();

            var userId = _userManager.GetUserId(User);

            if(User.IsInRole("Manager"))
            {
                Projects = _context.Projects
                .Where(c => c.CreatedById == userId)
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString()
                })
                .ToList();
            }
            else if (User.IsInRole("TeamMember"))
            {
                Projects = _context.ProjectMembers
                .Where(c => c.MemberId == userId)
                .Select(s => new SelectListItem
                {
                    Text = s.Project.Name,
                    Value = s.ProjectId.ToString()
                })
                .ToList();
            }

        }
    }
}