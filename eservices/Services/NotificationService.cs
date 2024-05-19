
using Pattern_of_life.Data;
using Pattern_of_life.Models.Entity;

namespace Pattern_of_life.Services
{
    public interface INotficationService
    {
        public List<CoreNotification> GetAllCoreNotification();
    }
    public class NotificationService : INotficationService
    {
        private readonly ApplicationDbContext _context;
        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CoreNotification> GetAllCoreNotification()
        => _context.CoreNotification.Where(item=>!item.IsRead).ToList();
    }
}
