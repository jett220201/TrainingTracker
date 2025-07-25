﻿using Microsoft.EntityFrameworkCore;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Infrastructure.Persistence;

namespace TrainingTracker.Infrastructure.Repository
{
    public class UserProgressesRepository : GenericRepository<UserProgress>, IUserProgressesRepository
    {
        private readonly IDbContextFactory<CoreDBContext> _context;
        public UserProgressesRepository(IDbContextFactory<CoreDBContext> context) : base(context)
        {
            _context = context;
        }
    }
}
