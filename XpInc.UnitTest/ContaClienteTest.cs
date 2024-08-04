using System;
using Xunit;
using FluentAssertions;
using FluentValidation.Results;
using XpInc.ContaCliente.API.Models.Entities;
namespace XpInc.UnitTest
{
    public class ContaClienteTest
    {
        [Fact]
        public void ContaClienteSaldo_DeveSerValida_QuandoTodosOsCamposForemValidos()
        {
            var contaClienteSaldo = new ContaClienteSaldo(
                nome: "João da Silva",
                telefone: "(11) 99999-9999",
                cpf: "123.456.789-09",
                saldoDisponivel: 1000.0,
                totalInvestido: 5000.0,
                id: Guid.NewGuid()
            );

            bool isValid = contaClienteSaldo.EhValido();

            isValid.Should().BeTrue();
            contaClienteSaldo.RetornaValidationResult().Errors.Should().BeEmpty();
        }

        [Fact]
        public void ContaClienteSaldo_DeveSerInvalida_QuandoClienteVierVazio()
        {
            var contaClienteSaldo = new ContaClienteSaldo(
                nome: "João da Silva",
                telefone: "(11) 99999-9999",
                cpf: "123.456.789-09",
                saldoDisponivel: 1000.0,
                totalInvestido: 5000.0,
                id: Guid.Empty
            );

            bool isValid = contaClienteSaldo.EhValido();

            isValid.Should().BeFalse();
            contaClienteSaldo.RetornaValidationResult().Errors.Should().Contain(x => x.PropertyName == "Id");
        }

        [Fact]
        public void ContaClienteSaldo_DeveSerInvalida_QuandoNomeVierVazio()
        {
            var contaClienteSaldo = new ContaClienteSaldo(
                nome: "",
                telefone: "(11) 99999-9999",
                cpf: "123.456.789-09",
                saldoDisponivel: 1000.0,
                totalInvestido: 5000.0,
                id: Guid.NewGuid()
            );

            bool isValid = contaClienteSaldo.EhValido();

            isValid.Should().BeFalse();
            contaClienteSaldo.RetornaValidationResult().Errors.Should().Contain(x => x.PropertyName == "NomeCompleto");
        }

        [Fact]
        public void ContaClienteSaldo_DeveSerInvalida_QuandoCPFInvalido()
        {
            var contaClienteSaldo = new ContaClienteSaldo(
                nome: "João da Silva",
                telefone: "(11) 99999-9999",
                cpf: "123.456.789-00",
                saldoDisponivel: 1000.0,
                totalInvestido: 5000.0,
                id: Guid.NewGuid()
            );

            bool isValid = contaClienteSaldo.EhValido();

            isValid.Should().BeFalse();
            contaClienteSaldo.RetornaValidationResult().Errors.Should().Contain(x => x.PropertyName == "CPF");
        }

        [Fact]
        public void ContaClienteSaldo_DeveSerInvalida_QuandoTelefoneCelularInvalido()
        {
            var contaClienteSaldo = new ContaClienteSaldo(
                nome: "João da Silva",
                telefone: "99999999",
                cpf: "123.456.789-09",
                saldoDisponivel: 1000.0,
                totalInvestido: 5000.0,
                id: Guid.NewGuid()
            );

            bool isValid = contaClienteSaldo.EhValido();

            isValid.Should().BeFalse();
            contaClienteSaldo.RetornaValidationResult().Errors.Should().Contain(x => x.PropertyName == "TelefoneCelular");
        }

    }
}