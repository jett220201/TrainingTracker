using Microsoft.EntityFrameworkCore;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Infrastructure.Persistence;

namespace TrainingTracker.Infrastructure.Repository
{
    public class RefreshTokensRepository : GenericRepository<RefreshToken>, IRefreshTokensRepository
    {
        private readonly IDbContextFactory<CoreDBContext> _context;
        public RefreshTokensRepository(IDbContextFactory<CoreDBContext> context) : base(context)
        {
            _context = context;
        }

        public Task<RefreshToken> GetByToken(string token)
        {
            var context = _context.CreateDbContext();
            return context.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(rt => rt.Token == token);
        }
    }

}
