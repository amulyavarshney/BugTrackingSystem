using BugTrackingSystem.DAL;
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
                .Select(bug => new BugViewModel
                {
                    BugId = bug.BugId,
                    Title = bug.Title,
                    State = bug.State,
                    Messages = bug.Messages.Select(m => new MessageViewModel
                    {
                        MessageId = m.MessageId,
                        Flag = m.Flag,
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
                    Flag = m.Flag,
                    Text = m.Text,
                }).ToList(),
            };
        }

        // create new message in a bug
        public async Task<MessageViewModel> CreateByIdAsync(int bugId, MessageCreateViewModel message)
        {
            var bug = await FromId(bugId);
            if(bug.State == BugState.RESOLVED)
            {
                throw new InvalidOperationException("Bug is already resolved.");
            }
            var m = new Message
            {
                Text = message.Text,
                BugId = bugId,
            };
            // update State
            var b = new BugUpdateViewModel { State = BugState.RESOLVED };
            UpdateAsync(bugId, b);

            await _context.Messages.AddAsync(m);
            await _context.SaveChangesAsync();
            var messageViewModel = new MessageViewModel
            {
                MessageId = m.MessageId,
                Flag = m.Flag,
                Text = m.Text,
            };
            return messageViewModel;
        }

        // update a bug status
        public async Task<BugViewModel> UpdateAsync(int bugId, BugUpdateViewModel bug)
        {
            var b = await FromId(bugId);
            var updatedBug = new Bug
            {
                BugId = bugId,
                Title = b.Title,
                State = bug.State,
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
                    Flag = m.Flag,
                    Text = m.Text,
                })
                .ToList();

            viewModel.Messages = messages;
            return viewModel;
        }

        private async Task<Bug> FromId(int id)
        {
            return await _context.Bugs.FirstAsync(bug => bug.BugId == id);
        }
    }
}
