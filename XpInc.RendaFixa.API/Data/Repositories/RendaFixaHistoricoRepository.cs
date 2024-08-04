using Microsoft.EntityFrameworkCore;
using XpInc.Core.Data;
using XpInc.RendaFixa.API.Data.Context;
using XpInc.RendaFixa.API.Models.Entities;
using XpInc.RendaFixa.API.Models.Interfaces;

namespace XpInc.RendaFixa.API.Data.Repositories
{
    public class RendaFixaHistoricoRepository : Repository<RendaFixaHistorico, RendaFixaContext>, IRendaFixaHistoricoRepository
    {
        public RendaFixaHistoricoRepository(RendaFixaContext context) : base(context)
        {
        }

        public async Task<IEnumerable<RendaFixaHistorico>> GetByProductId(Guid idProduto)
        {
            return await _dbSet.Where(x => x.ProdutoId == idProduto && x.Ativo).OrderByDescending(x => x.DataCadastro).ToArrayAsync();
        }
    }
}
