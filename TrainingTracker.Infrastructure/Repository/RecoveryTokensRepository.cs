using Microsoft.EntityFrameworkCore;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Infrastructure.Persistence;

namespace TrainingTracker.Infrastructure.Repository
{
    public class RecoveryTokensRepository : GenericRepository<RecoveryToken>, IRecoveryTokensRepository
    {
        private readonly IDbContextFactory<CoreDBContext> _context;
        public RecoveryTokensRepository(IDbContextFactory<CoreDBContext> context) : base(context)
        {
            _context = context;
        }

        public async Task<RecoveryToken> GetRecoveryTokenByToken(string token)
        {
            var context = await _context.CreateDbContextAsync();
            return await context.RecoveryTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }
    }
}
