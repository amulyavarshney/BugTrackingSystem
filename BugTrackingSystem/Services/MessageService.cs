using BugTrackingSystem.DAL;
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
                    Flag = m.Flag,
                    Text = m.Text,
                }).ToListAsync();
        }
        public async Task<MessageViewModel> GetByIdAsync(int messageId)
        {
            var message = await FromId(messageId);
            return ToViewModel(message);
        }

        private MessageViewModel ToViewModel(Message message)
        {
            return new MessageViewModel
            {
                MessageId = message.MessageId,
                Flag = message.Flag,
                Text = message.Text,
            };
        }
        private async Task<Message> FromId(int id)
        {
            return await _context.Messages.FirstAsync(m => m.MessageId == id);
        }
    }
}
