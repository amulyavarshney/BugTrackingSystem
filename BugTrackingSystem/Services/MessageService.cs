using BugTrackingSystem.DAL;
using BugTrackingSystem.Exceptions;
using BugTrackingSystem.Models;
using BugTrackingSystem.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BugTrackingSystem.Services
{
    public class MessageService : IMessageService
    {
        private readonly BugTrackingContext _context;
        public MessageService(BugTrackingContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<MessageViewModel>> GetAllAsync()
        {
            return await _context.Messages
                .Select(m => new MessageViewModel
                {
                    MessageId = m.MessageId,
                    IsResolved = m.IsResolved,
                    Text = m.Text,
                }).ToListAsync();
        }
        public async Task<MessageViewModel> GetByIdAsync(int messageId)
        {
            var message = await FromId(messageId);
            return ToViewModel(message);
        }
        public async Task<MessageViewModel> CreateAsync(int bugId, MessageCreateViewModel message)
        {
            // retrive the bug from the database
            var bug = await _context.Bugs.FirstAsync(bug => bug.BugId == bugId);
            if (bug == null)
            {
                throw new RecordNotFoundException($"Could not find the Bug with id: {bugId}");
            }

            // update bug status when first message is added
            if (bug.State == BugState.RESOLVED)
            {
                throw new InvalidOperationException("Bug is already resolved.");
            }

            var m = new Message
            {
                IsResolved = message.IsResolved,
                Text = message.Text,
                BugId = bugId,
            };

            // update bug state when message is marked as resolved.
            if (message.IsResolved == true)
            {
                bug.State = BugState.RESOLVED;
            }
            
            /*var b = new BugUpdateViewModel { State = BugState.RESOLVED };
            UpdateAsync(bugId, b);*/

            await _context.Messages.AddAsync(m);
            await _context.SaveChangesAsync();
            return ToViewModel(m);
        }
        private MessageViewModel ToViewModel(Message message)
        {
            return new MessageViewModel
            {
                MessageId = message.MessageId,
                IsResolved = message.IsResolved,
                Text = message.Text,
            };
        }
        private async Task<Message> FromId(int id)
        {
            var messageDb = await _context.Messages.FirstAsync(m => m.MessageId == id);
            if (messageDb == null)
            {
                throw new RecordNotFoundException($"Could not find the Message with id: {id}");
            }
            return messageDb;
        }
    }
}
