using FluentValidation;
using FluentValidation.Results;
using RabbitMQ.Client;
using System.Runtime.CompilerServices;
using XpInc.ContaCliente.API.Application.Commands;
using XpInc.Core.Domain;
using XpInc.Core.Utils;

namespace XpInc.ContaCliente.API.Models.Entities
{
    public class ContaClienteSaldo : Entity
    {
        public string NomeCompleto { get; set; }
        public string TelefoneCelular { get; set; }
        public string CPF { get; set; }
        public double SaldoDisponivel { get; set; }
        public double TotalInvestido { get; set; }
        public ContaClienteSaldo(string nome, string telefone, string cpf, double saldoDisponivel, double totalInvestido, Guid id)
        {
            Id = id;
            NomeCompleto = nome;
            TelefoneCelular = telefone;
            CPF = cpf;
            SaldoDisponivel = saldoDisponivel;
            TotalInvestido = totalInvestido;
        }
        public ContaClienteSaldo()
        {
            
        }
        private ValidationResult ValidationResult { get; set; }
        public bool EhValido()
        {
            ValidationResult = new ContaClienteSaldoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
        public ValidationResult RetornaValidationResult()
        {
            return ValidationResult;
        }
    }

    public class ContaClienteSaldoValidation : AbstractValidator<ContaClienteSaldo>
    {
        public ContaClienteSaldoValidation()
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
                    .Must(x => ValidaMascaras.ValidaCelular(x));
        }
    }
}
