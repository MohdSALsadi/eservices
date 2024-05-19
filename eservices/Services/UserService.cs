using eservices.Models;
using Pattern_of_life.Data;
using System.Data;

namespace eservices.Services
{
    public interface IUserService
    {
        int GetCountInActiveUsers();
    }
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int GetCountInActiveUsers()
        {
            var data = _context.Users.Count(e=>!e.IsActive);
            return data;
        }
    }
}
