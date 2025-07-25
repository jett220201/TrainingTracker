using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Repository
{
    public interface IRefreshTokensRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken> GetByToken(string token);
    }
}
