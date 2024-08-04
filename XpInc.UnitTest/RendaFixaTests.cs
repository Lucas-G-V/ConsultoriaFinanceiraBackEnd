using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpInc.RendaFixa.API.Models.Entities;
using XpInc.RendaFixa.API.Models.Enum;

namespace XpInc.UnitTest
{
    public class RendaFixaTests
    {
        [Fact]
        public void Deve_CriarProduto_Valido()
        {
            var produto = new RendaFixaProduto(
                Guid.NewGuid(),
                "Produto Teste",
                1000,
                50,
                true,
                DateTime.Now.AddYears(1),
                TipoTaxa.Fixa,
                5,
                1,
                Indexador.IPCA,
                FrequenciaPagamento.Mensal
            );

            produto.EhValido().Should().BeTrue();
        }

        [Fact]
        public void Nome_NaoPodeEstar_Vazio()
        {
            var produto = new RendaFixaProduto(
                Guid.NewGuid(),
                "",
                1000,
                50,
                true,
                DateTime.Now.AddYears(1),
                TipoTaxa.Fixa,
                5,
                1,
                Indexador.IPCA,
                FrequenciaPagamento.Mensal
            );

            produto.EhValido().Should().BeFalse();
            produto.RetornaValidationResult().Errors.Should().ContainSingle(e => e.ErrorMessage == "O nome do produto é obrigatório");
        }

        [Fact]
        public void ValorMinimo_DeveSer_MaiorQue_Zero()
        {
            var produto = new RendaFixaProduto(
                Guid.NewGuid(),
                "Produto Teste",
                0,
                50,
                true,
                DateTime.Now.AddYears(1),
                TipoTaxa.Fixa,
                5,
                1,
                Indexador.IPCA,
                FrequenciaPagamento.Mensal
            );

            produto.EhValido().Should().BeFalse();
            produto.RetornaValidationResult().Errors.Should().ContainSingle(e => e.ErrorMessage == "O valor mínimo deve ser maior que zero");
        }

        [Fact]
        public void ValorUnitario_DeveSerMaiorQue_Zero()
        {
            var produto = new RendaFixaProduto(
                Guid.NewGuid(),
                "Produto Teste",
                1000,
                0,
                true,
                DateTime.Now.AddYears(1),
                TipoTaxa.Fixa,
                5,
                1,
                Indexador.IPCA,
                FrequenciaPagamento.Mensal
            );

            produto.EhValido().Should().BeFalse();
            produto.RetornaValidationResult().Errors.Should().ContainSingle(e => e.ErrorMessage == "O valor unitário deve ser maior que zero");
        }

        [Fact]
        public void DataVencimento_DeveSer_Futura()
        {
            var produto = new RendaFixaProduto(
                Guid.NewGuid(),
                "Produto Teste",
                1000,
                50,
                true,
                DateTime.Now.AddDays(-1),
                TipoTaxa.Fixa,
                5,
                1,
                Indexador.IPCA,
                FrequenciaPagamento.Mensal
            );

            produto.EhValido().Should().BeFalse();
            produto.RetornaValidationResult().Errors.Should().ContainSingle(e => e.ErrorMessage == "A data de vencimento deve ser futura");
        }
    }
}
