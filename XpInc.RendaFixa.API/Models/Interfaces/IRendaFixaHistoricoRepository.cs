using XpInc.Core.Data;
using XpInc.RendaFixa.API.Data.Context;
using XpInc.RendaFixa.API.Models.Entities;

namespace XpInc.RendaFixa.API.Models.Interfaces
{
    public interface IRendaFixaHistoricoRepository : IRepository<RendaFixaHistorico, RendaFixaContext>
    {
        Task<IEnumerable<RendaFixaHistorico>> GetByProductId(Guid idProduto);
    }
}
