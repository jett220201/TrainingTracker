using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Services
{
    public class RecoveryTokensService : IRecoveryTokensService
    {
        private readonly IRecoveryTokensRepository _recoveryTokensRepository;
        public RecoveryTokensService(IRecoveryTokensRepository recoveryTokensRepository)
        {
            _recoveryTokensRepository = recoveryTokensRepository ?? throw new ArgumentNullException(nameof(recoveryTokensRepository));
        }
        public Task Add(RecoveryToken entity)
        {
            return _recoveryTokensRepository.Add(entity);
        }
        public Task AddRange(IEnumerable<RecoveryToken> entity)
        {
            return _recoveryTokensRepository.AddRange(entity);
        }
        public Task<RecoveryToken> AddReturn(RecoveryToken entity)
        {
            return _recoveryTokensRepository.AddReturn(entity);
        }
        public Task Delete(RecoveryToken entity)
        {
            return _recoveryTokensRepository.Delete(entity);
        }
        public Task<IEnumerable<RecoveryToken>> GetAll()
        {
            return _recoveryTokensRepository.GetAll();
        }
        public Task<RecoveryToken> GetById(int id)
        {
            return _recoveryTokensRepository.GetById(id);
        }
        public Task Update(RecoveryToken entity)
        {
            return _recoveryTokensRepository.Update(entity);
        }
        public Task<RecoveryToken> UpdateReturn(RecoveryToken entity)
        {
            return _recoveryTokensRepository.UpdateReturn(entity);
        }
    }
}
