using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using AuthTest.Data;
using AuthTest.Model;

namespace AuthTest.Pages.Manager
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ApplicationUser CurrentUser { get; set; }
        public List<TeamMemberTask> Tasks { get; set; }
        public IQueryable<TeamMemberTask> DueTodayQuery { get; set; }
        public List<TeamMemberTask> DueTodayTasks { get; set; }
        public IQueryable<TeamMemberTask> WeekTasksQuery { get; set; }
        public List<TeamMemberTask> WeekTasks { get; set; }
        public IQueryable<TeamMemberTask> OverdueTasksQuery { get; set; }
        public List<TeamMemberTask> OverdueTasks { get; set; }
        [BindProperty(SupportsGet = false, Name = "Filter")]
        public TaskFilter? Filter { get; set; } = new TaskFilter();

        public ICollection<Project> AllProjects { get; set; } = new List<Project>();
        public Project Project { get; set; }
        public Project CurrentProject { get; set; }
        public List<ProjectMember> CurrentProjectMembers { get; set; }
        public List<ApplicationUser> AllProjectMembers { get; set; }
        public List<SelectListItem> AllProjectMembersDropdown { get; set; }
        public List<Project> ManagerProjects { get; set; }
        public List<SelectListItem> ManagerProjectsDropdown { get; set; }

        [BindProperty(SupportsGet = false, Name = "NewProjectForm")]
        public CreateNewProject NewProjectForm { get; set; }

        public class CreateNewProject
        {
            [Required]
            [Display(Name = "Project Name")]
            [StringLength(200, MinimumLength = 3, ErrorMessage = "Project name must be between 3 and 200 characters.")]
            public string Name { get; set; }

            [Display(Name = "Project Description")]
            [StringLength(1000, MinimumLength = 3, ErrorMessage = "Description cannot exceed 1000 characters.")]
            public string? Description { get; set; }
        }

        [BindProperty(SupportsGet = false, Name = "InviteForm")]
        public InvitationForm InviteForm { get; set; }

        public List<GroupedInvitationCodes> InviteCodes { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SelectedProjectId { get; set; }

        [BindProperty(SupportsGet = false, Name = "CreateTaskForm")]
        public CreateNewTask CreateTaskForm { get; set; }

        public Project SelectedProject { get; set; }
        public List<ProjectMember> SelectedProjectMembers { get; set; }
        public List<SelectListItem> SelectedProjectMembersDropdown { get; set; }
        public List<SelectListItem> TasksPriority { get; set; }
        public List<SelectListItem> TasksStatus { get; set; }

        public class CreateNewTask
        {
            [Required] public int ProjectId { get; set; }

            [Required]
            [Display(Name = "Task Title")]
            [StringLength(400, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 400 characters.")]
            public string Title { get; set; }

            [StringLength(1000, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 400 characters.")]
            public string? Description { get; set; }

            [Required] public string AssignedToMemberId { get; set; }
            [Required] public DateTime DueDate { get; set; }
            [Required] public int PriorityId { get; set; }
        }

        public class InvitationForm
        {
            [Required] public int ProjectId { get; set; }

            [Required]
            [Display(Name = "Number of Codes")]
            public int? Number { get; set; }
        }

        public class GroupedInvitationCodes
        {
            public Project Project { get; set; }
            public List<int> Codes { get; set; }
        }

        public void OnGet(
            int? selectedStatusId = 0,
            int? selectedPriorityId = 0,
            string? selectedMemberId = "",
            int? selectedProjectId = 0)
        {
            var userId = _userManager.GetUserId(User);
            CurrentUser = _context.Users.FirstOrDefault(u => u.Id == userId);

            Tasks = _context.Tasks
                .Where(c => c.CreatedById == userId)
                .Include(t => t.AssignedToMember)
                .Include(r => r.Status)
                .Include(v => v.Priority)
                .ToList();

            var today = DateTime.Today;
            
            DueTodayQuery = _context.Tasks
            .Include(i => i.AssignedToMember)
                .Include(o => o.Project)
                .Include(d => d.Priority)
                .Include(t => t.Status)
            .Where(t => t.DueDate.Date == today);

            WeekTasksQuery = _context.Tasks
            .Include(i => i.AssignedToMember)
                .Include(o => o.Project)
                .Include(d => d.Priority)
                .Include(t => t.Status)
            .Where(t => t.DueDate.Date > today && t.DueDate.Date <= today.AddDays(7));

            OverdueTasksQuery = _context.Tasks
            .Include(i => i.AssignedToMember)
                .Include(o => o.Project)
                .Include(d => d.Priority)
                .Include(t => t.Status)
            .Where(t => t.DueDate.Date < today);

            // Apply filters based on the parameters if available
            if (selectedStatusId != 0)
            {
                DueTodayQuery = DueTodayQuery.Where(t => t.StatusId == selectedStatusId);
                WeekTasksQuery = WeekTasksQuery.Where(t => t.StatusId == selectedStatusId);
                OverdueTasksQuery = OverdueTasksQuery.Where(t => t.StatusId == selectedStatusId);
            }

            if(selectedPriorityId != 0)
            {
                DueTodayQuery = DueTodayQuery.Where(t => t.PriorityId == selectedPriorityId);
                WeekTasksQuery = WeekTasksQuery.Where(t => t.PriorityId == selectedPriorityId);
                OverdueTasksQuery = OverdueTasksQuery.Where(t => t.PriorityId == selectedPriorityId);
            }

            if (!String.IsNullOrEmpty(selectedMemberId))
            {
                DueTodayQuery = DueTodayQuery.Where(t => t.AssignedToMemberId == selectedMemberId);
                WeekTasksQuery = WeekTasksQuery.Where(t => t.AssignedToMemberId == selectedMemberId);
                OverdueTasksQuery = OverdueTasksQuery.Where(t => t.AssignedToMemberId == selectedMemberId);
            }

            if(selectedProjectId != 0)
            {
                DueTodayQuery = DueTodayQuery.Where(t => t.ProjectId == selectedProjectId);
                WeekTasksQuery = WeekTasksQuery.Where(t => t.ProjectId == selectedProjectId);
                OverdueTasksQuery = OverdueTasksQuery.Where(t => t.ProjectId == selectedProjectId);
            }

            DueTodayTasks = DueTodayQuery.ToList();
            WeekTasks = WeekTasksQuery.ToList();
            OverdueTasks = OverdueTasksQuery.ToList();

            // --------------------

            AllProjects = _context.Projects
                .Select(p => new
                {
                    Project = p,
                    Status = p.Status // This will handle nulls gracefully
                })
                .AsEnumerable()
                .Select(x => x.Project)
                .ToList();

            Project = _context.Projects
                .Include(p => p.ProjectMembers)
                    .ThenInclude(r => r.Member)
                .FirstOrDefault(p => p.ManagerId == userId && p.IsFinished == false);

            ManagerProjects = _context.Projects
                .Where(p => p.CreatedById == userId && p.IsFinished == false)
                .ToList();

            ManagerProjectsDropdown = ManagerProjects
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString()
                })
                .ToList();

                AllProjectMembers = _context.ProjectMembers
                    .Where(pm => pm.Project.ManagerId == userId)
                    .Select(pm => pm.Member)
                    .Distinct()
                    .Include(m => m.MemberProfile)
                        .ThenInclude(mp => mp.Tasks)
                    .ToList();

            AllProjectMembersDropdown = AllProjectMembers
                .Select(s => new SelectListItem
                {
                    Text = s.FullName,
                    Value = s.Id.ToString()
                })
                .ToList();

            CurrentProject = _context.Projects
                .FirstOrDefault(p => p.CreatedById == userId && p.IsFinished == false);

            CurrentProjectMembers = _context.ProjectMembers
                .Include(i => i.Member)
                    .ThenInclude(ti => ti.MemberProfile)
                .Where(c => c.ProjectId == CurrentProject.Id)
                .ToList();

            ManagerProjectsDropdown = _context.Projects
                .Where(p => p.CreatedById == userId && p.IsFinished == false)
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString()
                })
                .ToList();

            InviteCodes = _context.InvitationCodes
                .Include(i => i.Project)
                .Where(c => c.Project.ManagerId == userId)
                .GroupBy(g => g.Project)
                .Select(s => new GroupedInvitationCodes
                {
                    Project = s.Key,
                    Codes = s.Select(c => c.Code).ToList()
                })
                .ToList();

            TasksPriority = _context.TasksPriority
            .Select(tp => new SelectListItem
            {
                Text = tp.Name,
                Value = tp.Id.ToString()
            })
            .ToList();

            TasksStatus = _context.ProjectsStatus
            .Select(tp => new SelectListItem
            {
                Text = tp.Name,
                Value = tp.Id.ToString()
            })
            .ToList();

            // Handle project member dropdown dynamically
            if (SelectedProjectId.HasValue)
            {
                    SelectedProjectMembers = _context.ProjectMembers
                    .Include(i => i.Member)
                        .ThenInclude(ti => ti.MemberProfile)
                    .Where(c => c.ProjectId == SelectedProjectId.Value)
                    .ToList();

                SelectedProjectMembersDropdown = SelectedProjectMembers
                    .Select(s => new SelectListItem
                    {
                        Text = s.Member.FullName,
                        Value = s.MemberId.ToString()
                    })
                    .ToList();

            }
        }

        public void OnPost()
        {
            // Handle form submission if needed
        }

        public IActionResult OnPostCreateProject()
        {
            // Remove any modelstate entries that are NOT for NewProjectForm
            var keysToRemove = ModelState.Keys
                .Where(k => !k.StartsWith("NewProjectForm"))
                .ToList();

            foreach (var k in keysToRemove)
            {
                ModelState.Remove(k);
            }

            if (!TryValidateModel(NewProjectForm, nameof(NewProjectForm)))
            {
                return Page();
            }

            var project = new Project
            {
                Name = NewProjectForm.Name,
                Description = NewProjectForm.Description,
                CreatedById = _userManager.GetUserId(User),
                ManagerId = _userManager.GetUserId(User),
                IsFinished = false,
                StatusId = 1
            };

            _context.Projects.Add(project);
            _context.SaveChanges();

            return RedirectToPage("/Manager/Dashboard");
        }

        public IActionResult OnPostCreateTask()
        {
            var keysToRemove = ModelState.Keys
                .Where(k => !k.StartsWith("CreateTaskForm"))
                .ToList();

            foreach (var k in keysToRemove)
            {
                ModelState.Remove(k);
            }

            if (!TryValidateModel(CreateTaskForm, nameof(CreateTaskForm)))
            {
                return Page();
            }

            int projectId = int.Parse(Request.Query["SelectedProjectId"]);

            var task = new TeamMemberTask
            {
                Title = CreateTaskForm.Title,
                Description = CreateTaskForm.Description,
                ProjectId = projectId,
                CreatedById = _userManager.GetUserId(User),
                AssignedToMemberId = CreateTaskForm.AssignedToMemberId,
                DueDate = CreateTaskForm.DueDate,
                PriorityId = CreateTaskForm.PriorityId,
                StatusId = 1
            };

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return RedirectToPage("/Manager/Dashboard");
        }

        public IActionResult OnPostCreateInvitationCode()
        {
            var keysToRemove = ModelState.Keys
                .Where(k => !k.StartsWith("InviteForm"))
                .ToList();

            foreach (var k in keysToRemove)
            {
                ModelState.Remove(k);
            }

            if (!TryValidateModel(InviteForm, nameof(InviteForm)))
            {
                return Page();
            }

            int codesCount = InviteForm?.Number ?? 1; // Default to 1 if not specified
            for (int i = 0; i < codesCount; i++)
            {
                var code = new InvitationCode
                {
                    ProjectId = InviteForm.ProjectId,
                    Code = new Random().Next(10000, 99999) // Generate a random 5-digit code
                };
                _context.InvitationCodes.Add(code);
            }

            _context.SaveChanges();
            return RedirectToPage("/Manager/Dashboard");
        }

        public IActionResult OnPostFilterTasks()
        {
            var keysToRemove = ModelState.Keys
                .Where(k => !k.StartsWith("Filter"))
                .ToList();
                
            foreach (var k in keysToRemove)
            {
            ModelState.Remove(k);
            }

            if (!TryValidateModel(Filter, nameof(Filter)))
                return Page();

            return RedirectToPage("/Manager/Dashboard",
            new { selectedStatusId = Filter.SelectedStatusId,
            selectedPriorityId = Filter.SelectedPriorityId,
            selectedMemberId = Filter.SelectedMemberId,
            selectedProjectId = Filter.SelectedProjectId }
            );
        }
    }
}