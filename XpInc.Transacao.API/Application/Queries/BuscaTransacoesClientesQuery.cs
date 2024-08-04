using MediatR;
using XpInc.Transacao.API.Models.Entities;

namespace XpInc.Transacao.API.Application.Queries
{
    public class BuscaTransacoesClientesQuery : IRequest<IEnumerable<TransacaoCliente>>
    {
        public Guid IdCliente { get; set; }
        public BuscaTransacoesClientesQuery(Guid idCliente)
        {
            IdCliente = idCliente; 
        }
    }
}
