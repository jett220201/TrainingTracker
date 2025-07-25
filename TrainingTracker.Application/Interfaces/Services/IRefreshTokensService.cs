using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Services
{
    public interface IRefreshTokensService : IGenericService<RefreshToken>
    {
        Task<RefreshToken> GetByToken(string token);
    }
}
