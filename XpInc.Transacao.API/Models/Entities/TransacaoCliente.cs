using FluentValidation;
using FluentValidation.Results;
using XpInc.Core.Domain;
using XpInc.Transacao.API.Models.Enums;

namespace XpInc.Transacao.API.Models.Entities
{
    public class TransacaoCliente : Entity
    {
        public Guid ClienteId { get; set; }
        public Guid? ProdutoId { get; set; }
        public string? NomeProduto { get; set; }
        public TipoTransacao Tipo { get; set; }
        public StatusTransacao Status { get; set; }
        public DateTime DataTransacao { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }

        public TransacaoCliente(Guid clienteId, TipoTransacao tipo, StatusTransacao status, DateTime dataTransacao,
                     decimal? quantidade = null, decimal? valorUnitario = null, Guid? produtoId = null, decimal? valorTotal = null)
        {
            ClienteId = clienteId;
            Tipo = tipo;
            Status = status;
            DataTransacao = dataTransacao;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            ProdutoId = produtoId;
            if(valorTotal != null) ValorTotal = valorTotal.Value;
            AtribuiValoresDasTransacoesParaConta();
        }
        public TransacaoCliente()
        {
            
        }

        public void AtribuiValoresDasTransacoesParaConta()
        {
            if (Tipo == TipoTransacao.Deposito || Tipo == TipoTransacao.Saque)
            {
                ValorTotal = ValorTotal;
                Quantidade = null;
                ValorUnitario = null;   
            }
            else
            {
                if(Quantidade.HasValue && ValorUnitario.HasValue)
                {
                    ValorTotal = Quantidade.Value * ValorUnitario.Value;
                }
            }
        }
        private ValidationResult ValidationResult { get; set; }
        public bool EhValido()
        {
            ValidationResult = new TransacaoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
        public ValidationResult RetornaValidationResult()
        {
            return ValidationResult;
        }

        public bool VerificaSeTransacaoDeDebitoEhValida(IEnumerable<TransacaoCliente> transacoes, TransacaoCliente transacaoNova)
        {
            if (transacaoNova.Tipo == TipoTransacao.Venda || transacaoNova.Tipo == TipoTransacao.Deposito) return true;
           
            return (CalculaSaldoDisponivel(transacoes, transacaoNova)) >= 0; 
        }
        public decimal CalculaSaldoDisponivel(IEnumerable<TransacaoCliente> transacoes, TransacaoCliente transacaoNova)
        {
            var valorPositivo = transacoes.Where(x => x.Status == StatusTransacao.Concluida &&
             (x.Tipo == TipoTransacao.Deposito || x.Tipo == TipoTransacao.Venda)).Sum(x => x.ValorTotal);
            var valorNegativo = transacoes.Where(x => x.Status == StatusTransacao.Concluida &&
            (x.Tipo == TipoTransacao.Saque || x.Tipo == TipoTransacao.Compra)).Sum(x => x.ValorTotal);
            if(transacaoNova.Tipo == TipoTransacao.Deposito || transacaoNova.Tipo == TipoTransacao.Venda) return (valorPositivo - valorNegativo + transacaoNova.ValorTotal);
            else  return (valorPositivo - valorNegativo - transacaoNova.ValorTotal);
        }
        public bool VerificaSeTransacaoDeVendaEhValida(IEnumerable<TransacaoCliente> transacoes, TransacaoCliente transacaoNova)
        {
            var historicoProduto = transacoes.Where(x => x.ProdutoId == transacaoNova.ProdutoId && x.Status == StatusTransacao.Concluida);
            if(transacaoNova.ValorUnitario != null && transacaoNova.Quantidade != null)
            {
                var quantidadeCotasCompradas = historicoProduto.Where(x => x.Status == StatusTransacao.Concluida 
                && x.Tipo == TipoTransacao.Compra
                ).Sum(x => x.Quantidade);
                var quantidadeCotasVendidas = historicoProduto.Where(x => x.Status == StatusTransacao.Concluida
                && x.Tipo == TipoTransacao.Venda
                ).Sum(x => x.Quantidade);
                return (quantidadeCotasCompradas - transacaoNova.Quantidade - quantidadeCotasVendidas) >= 0;
            }
            return VerificaVendaQuandoProdutoNaoEPorCota();
        }

        public bool VerificaVendaQuandoProdutoNaoEPorCota()
        {
            //Precisaria validar valor atual da ação, não apenas a quantidade de cotas (Quando e valor marcado ao mercado)
            return true;
        }

        public (decimal, decimal) CalculaSaldoEValorInvestido(IEnumerable<TransacaoCliente> transacoes, TransacaoCliente transacaoNova)
        {
            
            var saldodisponivel = CalculaSaldoDisponivel(transacoes, transacaoNova);
            var valorInvestido = CalculaValorInvestido(transacoes, transacaoNova);
            return (saldodisponivel, valorInvestido);
        }

        public decimal CalculaValorInvestido(IEnumerable<TransacaoCliente> transacoes, TransacaoCliente transacaoNova)
        {
            var valorInvestido = transacoes.Where(x => x.Status == StatusTransacao.Concluida && x.Tipo == TipoTransacao.Compra)
                .Sum(x => x.ValorTotal);
            var valorRetirado = transacoes.Where(x => x.Status == StatusTransacao.Concluida && x.Tipo == TipoTransacao.Venda)
                .Sum(x => x.ValorTotal);
            if (transacaoNova.Tipo == TipoTransacao.Compra) return valorInvestido + transacaoNova.ValorTotal - valorRetirado;
            else if  (transacaoNova.Tipo == TipoTransacao.Venda) return valorInvestido - transacaoNova.ValorTotal - valorRetirado;
            else return valorInvestido - valorRetirado;
        }

    }

    public class TransacaoValidation : AbstractValidator<TransacaoCliente>
    {
        public TransacaoValidation()
        {
            RuleFor(c => c.ClienteId)
               .NotEqual(Guid.Empty)
               .WithMessage("Id do cliente inválido");

            RuleFor(c => c.DataTransacao)
                .NotEmpty()
                .GreaterThan(DateTime.MinValue)
                .WithMessage("Data da transação deve ser maior que a data mínima permitida");

            RuleFor(c => c.Tipo)
                .IsInEnum()
                .WithMessage("Tipo de transação inválido");

            RuleFor(c => c.Status)
                .IsInEnum()
                .WithMessage("Status de transação inválido");

            When(c => c.Tipo != TipoTransacao.Deposito && c.Tipo != TipoTransacao.Saque, () =>
            {
                RuleFor(c => c.Quantidade)
                    .NotNull()
                    .WithMessage("Quantidade deve ser fornecida para transações que não são depósito ou saque");

                RuleFor(c => c.ValorUnitario)
                    .NotNull()
                    .WithMessage("ValorUnitario deve ser fornecido para transações que não são depósito ou saque");

                RuleFor(c => c.ValorTotal)
                    .Equal(c => c.Quantidade.GetValueOrDefault() * c.ValorUnitario.GetValueOrDefault())
                    .WithMessage("ValorTotal deve ser igual ao produto de Quantidade e ValorUnitario");
                RuleFor(c => c.ProdutoId)
                    .NotEmpty()
                    .WithMessage("Preencha o produto");
            })
            .Otherwise(() =>
            {
                RuleFor(c => c.Quantidade)
                    .Null()
                    .WithMessage("Quantidade não deve ser fornecida para depósitos e saques");

                RuleFor(c => c.ValorUnitario)
                    .Null()
                    .WithMessage("ValorUnitario não deve ser fornecido para depósitos e saques");
            });
        }
    }
}
