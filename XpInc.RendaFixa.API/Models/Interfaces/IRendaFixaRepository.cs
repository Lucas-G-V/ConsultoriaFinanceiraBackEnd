using XpInc.Core.Data;
using XpInc.RendaFixa.API.Data.Context;
using XpInc.RendaFixa.API.Models.Entities;

namespace XpInc.RendaFixa.API.Models.Interfaces
{
    public interface IRendaFixaRepository : IRepository<RendaFixaProduto, RendaFixaContext>
    {
        Task<IEnumerable<RendaFixaProduto>> GetProximasAoVencimento(DateTime DataReferencia, int diasProximosVencimento);
    }
}
