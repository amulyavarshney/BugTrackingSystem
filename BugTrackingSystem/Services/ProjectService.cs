using Microsoft.EntityFrameworkCore;
using BugTrackingSystem.DAL;
using BugTrackingSystem.Models;
using BugTrackingSystem.ViewModels;
using BugTrackingSystem.Exceptions;

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
                    Bugs = project.Bugs
                    /*.Include(b => b.Messages)*/
                    .Select(b => new BugViewModel
                    {
                        BugId = b.BugId,
                        Title = b.Title,
                        State = b.State,
                        Messages = b.Messages.Select(m => new MessageViewModel
                        {
                            MessageId = m.MessageId,
                            IsResolved = m.IsResolved,
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
                    IsResolved = m.IsResolved,
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
            var projectDb = await _context.Projects.Include(project => project.Bugs).FirstAsync(project => project.ProjectId == id);
            if(projectDb == null)
            {
                throw new RecordNotFoundException($"Could not find the Project with id: {id}");
            }
            return projectDb;
        }
    }
}