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
    }
}
