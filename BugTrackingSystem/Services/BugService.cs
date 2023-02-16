using BugTrackingSystem.DAL;
using BugTrackingSystem.Exceptions;
using BugTrackingSystem.Models;
using BugTrackingSystem.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BugTrackingSystem.Services
{
    public class BugService : IBugService
    {
        private readonly BugTrackingContext _context;
        public BugService(BugTrackingContext context)
        {
            _context = context;
        }

        // get all bugs
        public async Task<IEnumerable<BugViewModel>> GetAllAsync()
        {
            return await _context.Bugs
                .Include(bug => bug.Messages)
                .Select(bug => new BugViewModel
                {
                    BugId = bug.BugId,
                    Title = bug.Title,
                    State = bug.State,
                    Messages = bug.Messages.Select(m => new MessageViewModel
                    {
                        MessageId = m.MessageId,
                        IsResolved = m.IsResolved,
                        Text = m.Text,
                    }).ToList(),
                }).ToListAsync();
        }

        // get all messages in a bug
        public async Task<BugViewModel> GetByIdAsync(int bugId)
        {
            var bug = await FromId(bugId);
            return new BugViewModel
            {
                BugId = bug.BugId,
                Title = bug.Title,
                State = bug.State,
                Messages = bug.Messages.Select(m => new MessageViewModel
                {
                    MessageId = m.MessageId,
                    IsResolved = m.IsResolved,
                    Text = m.Text,
                }).ToList(),
            };
        }

        public async Task<BugViewModel> CreateAsync(int projectId, BugCreateViewModel bug)
        {
            var b = new Bug
            {
                Title = bug.Title,
                ProjectId = projectId,
            };
            await _context.Bugs.AddAsync(b);
            await _context.SaveChangesAsync();
            return ToViewModel(b);
        }

        // update a bug status
        public async Task<BugViewModel> UpdateAsync(int bugId, BugUpdateViewModel bug)
        {
            if (bugId != bug.BugId)
            {
                throw new DomainInvariantException($"Discrepancy in the Bug {bugId} and {bug.BugId}");
            }
            var b = await FromId(bugId);
            var updatedBug = new Bug
            {
                BugId = bugId,
                Title = b.Title,
                State = b.State,
                ProjectId = b.ProjectId,
                Project = b.Project,
                Messages = b.Messages,
            };
            await _context.SaveChangesAsync();
            return ToViewModel(updatedBug);
        }

        private BugViewModel ToViewModel(Bug bug)
        {
            var viewModel = new BugViewModel
            {
                BugId = bug.BugId,
                Title = bug.Title,
                State = bug.State,
            };
            var messages = bug.Messages
                .Where(m => m.BugId == bug.BugId)
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

        private async Task<Bug> FromId(int id)
        {
            var bugDb = await _context.Bugs.FirstAsync(bug => bug.BugId == id);
            if (bugDb == null)
            {
                throw new RecordNotFoundException($"Could not find the Bug with id: {id}");
            }
            return bugDb;
        }
    }
}
