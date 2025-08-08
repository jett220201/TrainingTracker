using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Repository
{
    public interface IRecoveryTokensRepository : IGenericRepository<RecoveryToken>
    {
        Task<RecoveryToken> GetRecoveryTokenByToken(string token);
    }
}
