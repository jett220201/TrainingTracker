using Microsoft.EntityFrameworkCore;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Infrastructure.Persistence;

namespace TrainingTracker.Infrastructure.Repository
{
    public class UserProgressesRepository : GenericRepository<UserProgress>, IUserProgressesRepository
    {
        private readonly IDbContextFactory<CoreDBContext> _context;
        public UserProgressesRepository(IDbContextFactory<CoreDBContext> context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserProgress>> GetUserProgressByUser(int idUser)
        {
            var context = await _context.CreateDbContextAsync();
            return await context.UserProgresses
                .Where(up => up.UserId == idUser)
                .ToListAsync();
        }

        public async Task<UserProgress> GetLastProgressByUser(int idUser)
        {
            var context = await _context.CreateDbContextAsync();
            return await context.UserProgresses.OrderBy(x => x.Id)
                .LastOrDefaultAsync(up => up.UserId == idUser);
        }
    }
}
