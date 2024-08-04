using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpInc.Transacao.API.Models.Entities;
using XpInc.Transacao.API.Models.Enums;

namespace XpInc.UnitTest
{
    public class TransacaoTest
    {
        [Fact]
        public void Construtor_DeveDefinirValorTotalCorreto_ParaDepositoESaque()
        {
            var clienteId = Guid.NewGuid();
            var dataTransacao = DateTime.Now;

            var deposito = new TransacaoCliente(clienteId, TipoTransacao.Deposito, StatusTransacao.Pendente, dataTransacao, valorTotal: 100);
            var saque = new TransacaoCliente(clienteId, TipoTransacao.Saque, StatusTransacao.Pendente, dataTransacao, valorTotal: 50);

            deposito.ValorTotal.Should().Be(100);
            deposito.Quantidade.Should().BeNull();
            deposito.ValorUnitario.Should().BeNull();

            saque.ValorTotal.Should().Be(50);
            saque.Quantidade.Should().BeNull();
            saque.ValorUnitario.Should().BeNull();
        }

        [Fact]
        public void Construtor_DeveDefinirValorTotalCorreto_ParaOutrosTiposDeTransacoes()
        {
            var clienteId = Guid.NewGuid();
            var dataTransacao = DateTime.Now;
            decimal quantidade = 10;
            decimal valorUnitario = 15;

            var compra = new TransacaoCliente(clienteId, TipoTransacao.Compra, StatusTransacao.Pendente, dataTransacao, quantidade, valorUnitario);

            compra.ValorTotal.Should().Be(quantidade * valorUnitario);
            compra.Quantidade.Should().Be(quantidade);
            compra.ValorUnitario.Should().Be(valorUnitario);
        }

        [Fact]
        public void Construtor_DeveDefinirValorTotalCorreto_ParaTipoDiferenteDeDepositoESaque()
        {
            var clienteId = Guid.NewGuid();
            var dataTransacao = DateTime.Now;
            decimal quantidade = 5;
            decimal valorUnitario = 20;

            var venda = new TransacaoCliente(clienteId, TipoTransacao.Venda, StatusTransacao.Pendente, dataTransacao, quantidade, valorUnitario);

            venda.ValorTotal.Should().Be(quantidade * valorUnitario);
        }

        [Fact]
        public void EhValido_DeveRetornarFalse_ParaTransacaoInválida()
        {
            var transacaoInvalida = new TransacaoCliente(Guid.Empty, TipoTransacao.Compra, StatusTransacao.Pendente, DateTime.Now);

            var resultado = transacaoInvalida.EhValido();

            resultado.Should().BeFalse();
            transacaoInvalida.RetornaValidationResult().Errors.Should().ContainSingle(error => error.ErrorMessage == "Id do cliente inválido");
        }

        [Fact]
        public void EhValido_DeveRetornarTrue_ParaTransacaoValida()
        {
            var transacaoValida = new TransacaoCliente(Guid.NewGuid(), TipoTransacao.Compra, StatusTransacao.Pendente, DateTime.Now, 10, 15);

            var resultado = transacaoValida.EhValido();

            resultado.Should().BeTrue();
        }

        [Fact]
        public void EhValido_DeveRetornarFalse_QuandoValorTotalIncorreto()
        {
            var transacao = new TransacaoCliente(Guid.NewGuid(), TipoTransacao.Compra, StatusTransacao.Pendente, DateTime.Now, 10, 15);
            transacao.ValorTotal = 999; 

            var resultado = transacao.EhValido();

            resultado.Should().BeFalse();
            transacao.RetornaValidationResult().Errors.Should().ContainSingle(error => error.ErrorMessage == "ValorTotal deve ser igual ao produto de Quantidade e ValorUnitario");
        }

        [Fact]
        public void EhValido_DeveRetornarFalse_QuandoDataTransacaoEValida()
        {
            var transacao = new TransacaoCliente(Guid.NewGuid(), TipoTransacao.Compra, StatusTransacao.Pendente, DateTime.MinValue, 10, 15);

            var resultado = transacao.EhValido();

            resultado.Should().BeFalse();
            transacao.RetornaValidationResult().Errors.Should().ContainSingle(error => error.ErrorMessage == "Data da transação deve ser maior que a data mínima permitida");
        }

        [Fact]
        public void EhValido_DeveRetornarFalse_QuandoValorUnitarioNuloEQuantidadePreenchida()
        {
            var transacao = new TransacaoCliente(Guid.NewGuid(), TipoTransacao.Venda, StatusTransacao.Pendente, DateTime.Now, 10, null);

            var resultado = transacao.EhValido();

            resultado.Should().BeFalse();
            transacao.RetornaValidationResult().Errors.Should().ContainSingle(error => error.ErrorMessage == "ValorUnitario deve ser fornecido para transações que não são depósito ou saque");
        }
    }
}
