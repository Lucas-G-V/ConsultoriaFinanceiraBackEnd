using FluentValidation.Results;
using MediatR;
using XpInc.ContaCliente.API.Models.Entities;
using XpInc.ContaCliente.API.Models.Interfaces;
using XpInc.Core.Messages;

namespace XpInc.ContaCliente.API.Application.Commands.Handler
{
    public class ClienteCommandHandler : CommandHandler,
       IRequestHandler<CriarClienteCommand, ValidationResult>
    {
        private readonly IContaClienteSaldoRepository _repository;

        public ClienteCommandHandler(IContaClienteSaldoRepository clienteRepository)
        {
            _repository = clienteRepository;
        }

        public async Task<ValidationResult> Handle(CriarClienteCommand message, CancellationToken cancellationToken)
        {
            var cliente = new ContaClienteSaldo(message.NomeCompleto, message.TelefoneCelular, message.CPF, message.SaldoDisponivel, message.TotalInvestido, message.Id);
            if (!cliente.EhValido()) return cliente.RetornaValidationResult();
            var buscaCPF = await _repository.GetByCpf(cliente.CPF);
            if(buscaCPF != null)
            {
                AdicionarErro("Cpf já cadastrado");
                return ValidationResult;
            }
            await _repository.Add(cliente);
            return await PersistirDados(_repository.UnitOfWork);
        }
    }
}
