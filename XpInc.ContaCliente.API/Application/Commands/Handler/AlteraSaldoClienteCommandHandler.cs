using FluentValidation.Results;
using MediatR;
using XpInc.ContaCliente.API.Models.Entities;
using XpInc.ContaCliente.API.Models.Interfaces;
using XpInc.Core.Messages;

namespace XpInc.ContaCliente.API.Application.Commands.Handler
{
    public class AlteraSaldoClienteCommandHandler : CommandHandler,
        IRequestHandler<AlteraSaldoClienteCommand, ValidationResult>
    {
        private readonly IContaClienteSaldoRepository _repository;

        public AlteraSaldoClienteCommandHandler(IContaClienteSaldoRepository clienteRepository)
        {
            _repository = clienteRepository;
        }

        public async Task<ValidationResult> Handle(AlteraSaldoClienteCommand message, CancellationToken cancellationToken)
        {
            var cliente = await _repository.GetById(message.IdCliente);
            cliente.SaldoDisponivel = (double)message.SaldoDisponivel;
            cliente.TotalInvestido = (double)message.TotalInvestido;
            if (!cliente.EhValido()) return cliente.RetornaValidationResult();
            await _repository.Update(cliente);
            return await PersistirDados(_repository.UnitOfWork);
        }
    }
}

