using MediatR;
using XpInc.ContaCliente.API.Models.Entities;

namespace XpInc.ContaCliente.API.Application.Queries
{
    public class SaldoClienteQuery : IRequest<ContaClienteSaldo>
    {
        public Guid IdCliente { get; set; }
        public SaldoClienteQuery(Guid idCliente)
        {
            IdCliente = idCliente;
        }
    }
}
