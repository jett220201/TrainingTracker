using Microsoft.EntityFrameworkCore;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Infrastructure.Persistence;

namespace TrainingTracker.Infrastructure.Repository
{
    public class RefreshTokensRepository : GenericRepository<RefreshToken>
    {
        private readonly IDbContextFactory<CoreDBContext> _context;
        public RefreshTokensRepository(IDbContextFactory<CoreDBContext> context) : base(context)
        {
            _context = context;
        }
    }

}
