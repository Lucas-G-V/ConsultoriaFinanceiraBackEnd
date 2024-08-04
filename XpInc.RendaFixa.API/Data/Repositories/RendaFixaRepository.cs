using Microsoft.EntityFrameworkCore;
using XpInc.Core.Data;
using XpInc.RendaFixa.API.Data.Context;
using XpInc.RendaFixa.API.Models.Entities;
using XpInc.RendaFixa.API.Models.Interfaces;

namespace XpInc.RendaFixa.API.Data.Repositories
{
    public class RendaFixaRepository : Repository<RendaFixaProduto, RendaFixaContext>, IRendaFixaRepository
    {
        public RendaFixaRepository(RendaFixaContext context) : base(context)
        {
        }

        public async Task<IEnumerable<RendaFixaProduto>> GetProximasAoVencimento(DateTime DataReferencia, int diasProximosVencimento)
        {
            return await _dbSet.Where(x => x.DataVencimento.Date >= DataReferencia.Date && x.DataVencimento.Date < DataReferencia.Date.AddDays(diasProximosVencimento)).ToArrayAsync();
        }
    }
}
