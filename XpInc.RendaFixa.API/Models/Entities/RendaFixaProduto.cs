using FluentValidation;
using FluentValidation.Results;
using XpInc.Core.Domain;
using XpInc.RendaFixa.API.Models.Enum;

namespace XpInc.RendaFixa.API.Models.Entities
{
    public class RendaFixaProduto : Entity
    {
        public Guid UsuarioCadastroId { get; set; }
        public string Nome { get; set; }
        public decimal ValorMinimo { get; set; }
        public decimal ValorUnitario { get; set; }
        public bool BaseadoEmCotas { get; set; }
        public DateTime DataVencimento { get; set; }
        public TipoTaxa TipoTaxa { get; set; }
        public decimal TaxaAnual { get; set; }
        public decimal TaxaAdicional { get; set; }
        public Indexador Indexador { get; set; }
        public FrequenciaPagamento Frequencia { get; set; }
        public int? QuantidadeCotasInicial { get; set; }
        public int? QuantidadeCotasDisponivel { get; set; }
        public string? EmailAdministrador { get; set; }
        public RendaFixaProduto(Guid usuarioCadastroId, string nome, decimal valorMinimo, decimal valorUnitario,
            bool baseadoEmCotas, DateTime dataVencimento, TipoTaxa tipoTaxa, decimal taxaAnual,
            decimal taxaAdicional, Indexador indexador, FrequenciaPagamento frequencia, int? quantidadeCotasInicial = null, 
            int? quantidadeCotasDisponivel = null)
        {
            UsuarioCadastroId = usuarioCadastroId;
            Nome = nome;
            ValorMinimo = valorMinimo;
            ValorUnitario = valorUnitario;
            BaseadoEmCotas = baseadoEmCotas;
            DataVencimento = dataVencimento;
            TipoTaxa = tipoTaxa;
            TaxaAnual = taxaAnual;
            TaxaAdicional = taxaAdicional;
            Indexador = indexador;
            Frequencia = frequencia;
            QuantidadeCotasDisponivel = quantidadeCotasDisponivel;
            QuantidadeCotasInicial = quantidadeCotasInicial;
        }
        public RendaFixaProduto()
        {
            
        }

        private ValidationResult ValidationResult { get; set; }
        public bool EhValido()
        {
            ValidationResult = new RendaFixaProdutoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
        public ValidationResult RetornaValidationResult()
        {
            return ValidationResult;
        }

        public void DebitaQuantidadeDeCotas(int quantidadeDeCotas)
        {
            if (BaseadoEmCotas)
            {
                QuantidadeCotasDisponivel = QuantidadeCotasDisponivel - quantidadeDeCotas;
            }
        }
    }
    public class RendaFixaProdutoValidation : AbstractValidator<RendaFixaProduto>
    {
        public RendaFixaProdutoValidation()
        {
            RuleFor(c => c.UsuarioCadastroId)
               .NotEqual(Guid.Empty)
               .WithMessage("Id do usuário inválido");

            RuleFor(c => c.Nome)
                .NotEmpty()
                .WithMessage("O nome do produto é obrigatório");

            RuleFor(c => c.ValorMinimo)
                .GreaterThan(0)
                .WithMessage("O valor mínimo deve ser maior que zero");

            RuleFor(c => c.ValorUnitario)
                .GreaterThan(0)
                .WithMessage("O valor unitário deve ser maior que zero");

            RuleFor(c => c.TaxaAnual)
                .GreaterThanOrEqualTo(0)
                .WithMessage("A taxa anual não pode ser negativa");

            RuleFor(c => c.TaxaAdicional)
                .GreaterThanOrEqualTo(0)
                .WithMessage("A taxa adicional não pode ser negativa");

            RuleFor(c => c.DataVencimento)
                .GreaterThan(DateTime.Now)
                .WithMessage("A data de vencimento deve ser futura");

            RuleFor(c => c.Indexador)
                .IsInEnum()
                .WithMessage("Indexador inválido");

            RuleFor(c => c.Frequencia)
                .IsInEnum()
                .WithMessage("Frequência inválida");

            When(c => c.BaseadoEmCotas, () =>
            {
                RuleFor(c => c.QuantidadeCotasDisponivel)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("A quantidade de cotas disponível deve ser maior ou igual a zero");
            });

        }
    }

}
