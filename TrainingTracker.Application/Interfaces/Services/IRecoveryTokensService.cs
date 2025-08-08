using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Services
{
    public interface IRecoveryTokensService : IGenericService<RecoveryToken>
    {
        Task<RecoveryToken> GetRecoveryTokenByToken(string token);
    }
}
