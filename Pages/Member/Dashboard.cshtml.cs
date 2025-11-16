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
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUser CurrentUser { get; set; }

        public List<TeamMemberTask> Tasks { get; set; }
        [BindProperty(SupportsGet = false, Name = "Filter")]
        public TaskFilter Filter { get; set; }
        public IQueryable<TeamMemberTask> OverdueTasksQuery { get; set; }
        public List<TeamMemberTask> OverDueTasks { get; set; }
        public IQueryable<TeamMemberTask> DueTodayQuery { get; set; }
        public List<TeamMemberTask> TodayTasks { get; set; }
        public IQueryable<TeamMemberTask> WeekTasksQuery { get; set; }
        public List<TeamMemberTask> ThisWeekTasks { get; set; }
        public IQueryable<TeamMemberTask> UpcomingTasksQuery { get; set; }
        public List<TeamMemberTask> UpcomingTasks { get; set; }
        public IQueryable<TeamMemberTask> CompletedTasksQuery { get; set; }
        public List<TeamMemberTask> CompletedTasks { get; set; }

        public List<Project> AllProjects { get; set; }
        public List<ProjectMember> MyProjects { get; set; }
        public List<SelectListItem> MyProjectsDropdown { get; set; }

        public List<SelectListItem> TasksStatus { get; set; }
        public List<SelectListItem> TasksPriority { get; set; }

        [BindProperty]
        public JoinProjectForm JoinProjectData { get; set; }

        public class JoinProjectForm
        {
            public int SelectedProjectId { get; set; }
            [Display(Name = "Invitation Code")]
            public string InvitationCode { get; set; }
        }

        public DashboardModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void OnGet(int selectedStatusId = 0,
            int selectedPriorityId = 0,
            int selectedProjectId = 0)
        {
            var userId = _userManager.GetUserId(User);
            CurrentUser = _context.Users.FirstOrDefault(u => u.Id == userId);

            var baseQuery = _context.Tasks
                .Where(c => c.AssignedToMemberId == userId)
                .Include(t => t.AssignedToMember)
                .Include(r => r.Status)
                .Include(v => v.Priority);
                // .Where(t => t.IsCompleted == false);

            Tasks = baseQuery.ToList();

            var today = DateTime.Today;

            OverdueTasksQuery = baseQuery
                .Include(o => o.Project)
                .Include(d => d.Priority)
                .Include(t => t.Status)
            .Where(c => c.DueDate.Date < today)
            .AsQueryable();

            DueTodayQuery = baseQuery
                .Include(o => o.Project)
                .Include(d => d.Priority)
                .Include(t => t.Status)
            .Where(c => c.DueDate.Date == today)
            .AsQueryable();

            WeekTasksQuery = baseQuery
                .Include(o => o.Project)
                .Include(d => d.Priority)
                .Include(t => t.Status)
            .Where(c => c.DueDate.Date >= today && c.DueDate.Date <= today.AddDays(7))
            .AsQueryable();

            UpcomingTasksQuery = baseQuery
                .Include(o => o.Project)
                .Include(d => d.Priority)
                .Include(t => t.Status)
                .Where(c => c.DueDate.Date > DateTime.Now)
            .AsQueryable();

            CompletedTasksQuery = baseQuery
                .Include(o => o.Project)
                .Include(d => d.Priority)
                .Include(t => t.Status)
                .Where(c => c.IsCompleted == true)
            .AsQueryable();

            if (selectedStatusId != 0)
            {
                DueTodayQuery = DueTodayQuery.Where(t => t.StatusId == selectedStatusId);
                WeekTasksQuery = WeekTasksQuery.Where(t => t.StatusId == selectedStatusId);
                OverdueTasksQuery = OverdueTasksQuery.Where(t => t.StatusId == selectedStatusId);
                UpcomingTasksQuery = UpcomingTasksQuery.Where(t => t.StatusId == selectedStatusId);
                CompletedTasksQuery = CompletedTasksQuery.Where(t => t.StatusId == selectedStatusId);
            }  
            if (selectedPriorityId != 0)
            {
                DueTodayQuery = DueTodayQuery.Where(t => t.PriorityId == selectedPriorityId);
                WeekTasksQuery = WeekTasksQuery.Where(t => t.PriorityId == selectedPriorityId);
                OverdueTasksQuery = OverdueTasksQuery.Where(t => t.PriorityId == selectedPriorityId);
                UpcomingTasksQuery = UpcomingTasksQuery.Where(t => t.PriorityId == selectedPriorityId);
                CompletedTasksQuery = CompletedTasksQuery.Where(t => t.PriorityId == selectedPriorityId);
            }  
            if (selectedProjectId != 0)
            {
                DueTodayQuery = DueTodayQuery.Where(t => t.ProjectId == selectedProjectId);
                WeekTasksQuery = WeekTasksQuery.Where(t => t.ProjectId == selectedProjectId);
                OverdueTasksQuery = OverdueTasksQuery.Where(t => t.ProjectId == selectedProjectId);
                UpcomingTasksQuery = UpcomingTasksQuery.Where(t => t.ProjectId == selectedProjectId);
                CompletedTasksQuery = CompletedTasksQuery.Where(t => t.ProjectId == selectedProjectId);
            }

            TodayTasks = DueTodayQuery.ToList();
            ThisWeekTasks = WeekTasksQuery.ToList();
            OverDueTasks = OverdueTasksQuery.ToList();
            UpcomingTasks = UpcomingTasksQuery.ToList();
            CompletedTasks = CompletedTasksQuery.ToList();

            AllProjects = _context.Projects.ToList();
            MyProjects = _context.ProjectMembers
                        .Include(i => i.Project)
                            .ThenInclude(r => r.Status)
                        .Where(c => c.MemberId == userId)
                        .ToList();

            MyProjectsDropdown = MyProjects
                .Select(s => new SelectListItem
                {
                    Text = s.Project.Name,
                    Value = s.ProjectId.ToString()
                })
                .ToList();

            TasksPriority = _context.TasksPriority
            .Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToList();

            TasksStatus = _context.TasksStatus
            .Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToList();
        }

        public void OnPost()
        {

        }
        public IActionResult OnPostJoinProject()
        {
            if (!ModelState.IsValid)
                return Page();

            var invitationCode = _context.InvitationCodes.Include(i => i.Project)
                    .FirstOrDefault(ic => ic.ProjectId == JoinProjectData.SelectedProjectId && ic.Code.ToString() == JoinProjectData.InvitationCode);

            if (invitationCode == null)
                return RedirectToPage("/NotFound");

            var userId = _userManager.GetUserId(User);

            _context.ProjectMembers.Add(new ProjectMember
            {
                ProjectId = JoinProjectData.SelectedProjectId,
                MemberId = userId
            });

            _context.InvitationCodes.Remove(invitationCode);
            _context.SaveChanges();

            return RedirectToPage("/Member/Dashboard");
        }

        public IActionResult OnPostFilterTask()
        {
            // Remove any modelstate entries that are NOT for NewProjectForm
            var keysToRemove = ModelState.Keys
                .Where(k => !k.StartsWith("Filter"))
                .ToList();

            foreach (var k in keysToRemove)
            {
                ModelState.Remove(k);
            }

            if (!TryValidateModel(Filter, nameof(Filter)))
            {
                return Page();
            }

        return RedirectToPage("/Member/Dashboard",
        new { selectedStatusId = Filter.SelectedStatusId,
        selectedPriorityId = Filter.SelectedPriorityId,
        selectedProjectId = Filter.SelectedProjectId});
        }
    }
}
