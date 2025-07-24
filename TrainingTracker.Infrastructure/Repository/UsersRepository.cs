using Microsoft.EntityFrameworkCore;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Infrastructure.Persistence;

namespace TrainingTracker.Infrastructure.Repository
{
    public class UsersRepository : GenericRepository<User>
    {
        private readonly IDbContextFactory<CoreDBContext> _context;
        public UsersRepository(IDbContextFactory<CoreDBContext> context) : base(context)
        {
            _context = context;
        }
    }
}
