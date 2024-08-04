using AutoMapper;
using FluentValidation.Results;
using MediatR;
using XpInc.ApiConfig.Services;
using XpInc.Core.Messages;
using XpInc.Transacao.API.Models.Interfaces;


namespace XpInc.Transacao.API.Application.Commands.Handlers
{
    public class AlteraStatusTransacaoCommandHandler : CommandHandler,
         IRequestHandler<AlteraStatusTransacaoCommand, ValidationResult>
    {
        private readonly ITransacaoRepository _repository;

        public AlteraStatusTransacaoCommandHandler(ITransacaoRepository repository)
        {
            _repository = repository;
        }

        public async Task<ValidationResult> Handle(AlteraStatusTransacaoCommand message, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(message.Id);
            entity.Status = message.Status;
            if (!entity.EhValido()) return entity.RetornaValidationResult();
            await _repository.Update(entity);
            return await PersistirDados(_repository.UnitOfWork);
        }
    }
}
