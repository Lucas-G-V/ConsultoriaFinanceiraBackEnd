using MediatR;
using XpInc.Cache;
using XpInc.ContaCliente.API.Models.Entities;
using XpInc.ContaCliente.API.Models.Interfaces;
using XpInc.Core.Data;

namespace XpInc.ContaCliente.API.Application.Queries.Handler
{
    public class SaldoClienteQueryHandler : IRequestHandler<SaldoClienteQuery,ContaClienteSaldo>
    {
        private readonly IContaClienteSaldoRepository _repository;

        public SaldoClienteQueryHandler(IContaClienteSaldoRepository repository)
        {
            _repository = repository;
        }

        public async Task<ContaClienteSaldo> Handle(SaldoClienteQuery request, CancellationToken cancellationToken)
        {
            var cliente = await _repository.GetById(request.IdCliente);
            return cliente;
        }
    }
}
