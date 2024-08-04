using FluentValidation;
using XpInc.Core.Messages;
using XpInc.Core.Utils;

namespace XpInc.ContaCliente.API.Application.Commands
{
    public class CriarClienteCommand : Command
    {
        public Guid Id { get; set; }
        public string NomeCompleto { get; set; }
        public string TelefoneCelular { get; set; }
        public string CPF { get; set; }
        public double SaldoDisponivel { get; set; } = 0;
        public double TotalInvestido { get; set; } = 0;
        public CriarClienteCommand(string nome, string telefoneCelular, string cpf, Guid id)
        {
            Id = id;
            AggregateId = id;
            NomeCompleto = nome;
            CPF = cpf;
            TelefoneCelular = telefoneCelular;
        }
        public CriarClienteCommand()
        {
            Id = AggregateId;
        }
        public override bool EhValido()
        {
            ValidationResult = new CriarClienteValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CriarClienteValidation : AbstractValidator<CriarClienteCommand>
    {
        public CriarClienteValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente inválido");

            RuleFor(c => c.NomeCompleto)
                .NotEmpty()
                .WithMessage("O nome do cliente não foi informado");

            RuleFor(c => c.CPF)
                .Must(x => ValidaMascaras.ValidaCPF(x))
                .WithMessage("O CPF informado não é válido.");

            RuleFor(c => c.TelefoneCelular)
                .Must(x => ValidaMascaras.ValidaCelular(x))
                .WithMessage("O celular informado não é válido.");
        }
    }
}
