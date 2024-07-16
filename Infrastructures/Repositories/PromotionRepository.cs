using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class PromotionRepository : GenericRepository<Promotion>, IPromotionRepository
    {
        private readonly AppDbContext _dbContext;
        public PromotionRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }
        public async Task<bool> HardDelete (int id)
        {
           var promotion = await _dbContext.Promotions.FindAsync(id);
            if(promotion == null)
            {
                return false;
            }
            _dbContext.Promotions.Remove(promotion);
            return true;
        }
    }
}
