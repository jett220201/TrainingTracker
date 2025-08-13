using Microsoft.EntityFrameworkCore;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Infrastructure.Persistence;

namespace TrainingTracker.Infrastructure.Repository
{
    public class UsersRepository : GenericRepository<User>, IUsersRepository
    {
        private readonly IDbContextFactory<CoreDBContext> _context;
        public UsersRepository(IDbContextFactory<CoreDBContext> context) : base(context)
        {
            _context = context;
        }

        public Task<User> AuthenticateAsync(string username, string password)
        {
            var context = _context.CreateDbContext();
            return context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == password);
        }

        public Task<User> GetUserByUserName(string username)
        {
            var context = _context.CreateDbContext();
            return context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
        }
        public Task<User> GetUserByEmail(string email)
        {
            var context = _context.CreateDbContext();
            return context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserById(int id)
        {
            var context = await _context.CreateDbContextAsync();
            return await context.Set<User>()
                .AsNoTracking()
                .Include(u => u.Workouts)
                .ThenInclude(w => w.WorkoutExercises)
                .Include(u => u.Goals)
                .Include(u => u.UserProgresses)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
