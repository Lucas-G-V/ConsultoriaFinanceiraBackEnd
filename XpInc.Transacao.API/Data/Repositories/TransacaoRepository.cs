using Microsoft.EntityFrameworkCore;
using XpInc.Core.Data;
using XpInc.Transacao.API.Data.Contexts;
using XpInc.Transacao.API.Models.Entities;
using XpInc.Transacao.API.Models.Enums;
using XpInc.Transacao.API.Models.Interfaces;

namespace XpInc.Transacao.API.Data.Repositories
{
    public class TransacaoRepository : Repository<TransacaoCliente, TransacaoContext>, ITransacaoRepository
    {
        public TransacaoRepository(TransacaoContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TransacaoCliente>> GetByIdCliente(Guid idCliente)
        {
            return await _dbSet.Where(x => x.Ativo == true && x.Status == StatusTransacao.Concluida && x.ClienteId == idCliente)
                .OrderByDescending(x => x.DataTransacao)
                .ToArrayAsync();
        }
    }
}
