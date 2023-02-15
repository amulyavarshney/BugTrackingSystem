using Microsoft.EntityFrameworkCore;
using BugTrackingSystem.DAL;
using BugTrackingSystem.Models;
using BugTrackingSystem.ViewModels;

namespace BugTrackingSystem.Services
{
    public class ProjectService : IProjectService
    {
        private readonly BugTrackingContext _context;
        public ProjectService(BugTrackingContext context)
        {
            _context = context;
        }

        // get all projects
        public async Task<IEnumerable<ProjectViewModel>> GetAllAsync()
        {
            return await _context.Projects
                .Include(project => project.Bugs)
                .Select(project => new ProjectViewModel
                {
                    ProjectId = project.ProjectId,
                    Title = project.Title,
                    Bugs = project.Bugs.Select(b => new BugViewModel
                    {
                        BugId = b.BugId,
                        Title = b.Title,
                        State = b.State,
                        Messages = b.Messages.Select(m => new MessageViewModel
                        {
                            MessageId = m.MessageId,
                            Flag = m.Flag,
                            Text = m.Text,
                        }).ToList(),
                    }).ToList(),
                }).ToListAsync();
        }

        // get project by id
        public async Task<ProjectViewModel> GetByIdAsync(int projectId)
        {
            var project = await FromId(projectId);
            return ToViewModel(project);
        }

        // create a new project
        public async Task<ProjectViewModel> CreateAsync(ProjectCreateViewModel project)
        {
            var p = ToEntity(project);
            await _context.Projects.AddAsync(p);
            await _context.SaveChangesAsync();
            return ToViewModel(p);
        }

        // create new bug in a project
        public async Task<BugViewModel> CreateByIdAsync(int projectId, BugCreateViewModel bug)
        {
            var project = await FromId(projectId);
            var b = new Bug
            {
                Title = bug.Title,
                ProjectId = projectId,
            };
            await _context.Bugs.AddAsync(b);
            await _context.SaveChangesAsync();
            return ToBugViewModel(b);
        }

        // delete a project
        public async Task DeleteAsync(int projectId)
        {
            var project = await FromId(projectId);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
        private ProjectViewModel ToViewModel(Project project)
        {
            var viewModel = new ProjectViewModel
            {
                ProjectId = project.ProjectId,
                Title = project.Title,
            };
            var bugs = project.Bugs
                .Where(b => b.ProjectId == project.ProjectId)
                .Select(b => ToBugViewModel(b))
                .ToList();

            viewModel.Bugs = bugs;
            return viewModel;
        }

        private BugViewModel ToBugViewModel(Bug bug)
        {
            var viewModel = new BugViewModel
            {
                BugId = bug.BugId,
                Title = bug.Title,
                State = bug.State,
            };
            var messages = bug.Messages
                .Where(m => m.MessageId == m.MessageId)
                .Select(m => new MessageViewModel
                {
                    MessageId = m.MessageId,
                    Flag = m.Flag,
                    Text = m.Text,
                })
                .ToList();

            viewModel.Messages = messages;
            return viewModel;
        }
        private Project ToEntity(ProjectCreateViewModel project)
        {
            return new Project
            {
                Title = project.Title,
            };
        }
        private async Task<Project> FromId(int id)
        {
            return await _context.Projects.Include(project => project.Bugs).FirstAsync(project => project.ProjectId == id);
        }
    }
}