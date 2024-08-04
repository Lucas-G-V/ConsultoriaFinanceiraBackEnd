using FluentValidation;
using FluentValidation.Results;
using RabbitMQ.Client;
using XpInc.Core.Domain;

namespace XpInc.RendaFixa.API.Models.Entities
{
    public class RendaFixaHistorico : Entity
    {
        public Guid ClienteId { get; set; }
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; }
        public decimal ValorMinimo { get; set; }
        public decimal ValorUnitario { get; set; }
        private ValidationResult ValidationResult { get; set; }
        public RendaFixaHistorico(Guid clientId, Guid produtoId, string nome, decimal valorMinimo, decimal valorUnitario)
        {
            ClienteId = clientId;
            ProdutoId = produtoId;
            Nome = nome;
            ValorMinimo = valorMinimo;
            ValorUnitario = valorUnitario;
        }
        public RendaFixaHistorico()
        {

        }

        public bool EhValido()
        {
            ValidationResult = new RendaFixaHistoricoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class RendaFixaHistoricoValidation : AbstractValidator<RendaFixaHistorico>
    {
        public RendaFixaHistoricoValidation()
        {
            RuleFor(c => c.ClienteId)
               .NotEqual(Guid.Empty)
               .WithMessage("Id do Admin Inválido");
            RuleFor(c => c.ProdutoId)
               .NotEqual(Guid.Empty)
               .WithMessage("Id do produto inválido");

            RuleFor(c => c.Nome)
                .NotEmpty()
                .WithMessage("O nome do produto é obrigatório");

            RuleFor(c => c.ValorMinimo)
                .GreaterThan(0)
                .WithMessage("O valor mínimo deve ser maior que zero");

            RuleFor(c => c.ValorUnitario)
                .GreaterThan(0)
                .WithMessage("O valor unitário deve ser maior que zero");

        }
    }
}
