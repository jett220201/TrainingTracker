using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Services
{
    public class RefreshTokensService : IRefreshTokensService
    {
        private readonly IRefreshTokensRepository _refreshTokensRepository;
        
        public RefreshTokensService(IRefreshTokensRepository refreshTokensRepository)
        {
            _refreshTokensRepository = refreshTokensRepository ?? throw new ArgumentNullException(nameof(refreshTokensRepository));
        }

        public Task Add(RefreshToken entity)
        {
            return _refreshTokensRepository.Add(entity);
        }

        public Task AddRange(IEnumerable<RefreshToken> entity)
        {
            return _refreshTokensRepository.AddRange(entity);
        }

        public Task<RefreshToken> AddReturn(RefreshToken entity)
        {
            return _refreshTokensRepository.AddReturn(entity);
        }

        public Task Delete(RefreshToken entity)
        {
            return _refreshTokensRepository.Delete(entity);
        }

        public Task<IEnumerable<RefreshToken>> GetAll()
        {
            return _refreshTokensRepository.GetAll();
        }

        public Task<RefreshToken> GetById(int id)
        {
            return _refreshTokensRepository.GetById(id);
        }

        public Task Update(RefreshToken entity)
        {
            return _refreshTokensRepository.Update(entity);
        }

        public Task<RefreshToken> UpdateReturn(RefreshToken entity)
        {
            return _refreshTokensRepository.UpdateReturn(entity);
        }
    }
}
