using XpInc.Core.Data;
using XpInc.Transacao.API.Data.Contexts;
using XpInc.Transacao.API.Models.Entities;

namespace XpInc.Transacao.API.Models.Interfaces
{
    public interface ITransacaoRepository : IRepository<TransacaoCliente, TransacaoContext>
    {
        Task<IEnumerable<TransacaoCliente>> GetByIdCliente(Guid idCliente);
    }
}
