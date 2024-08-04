using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpInc.Core.Messages.IntegrationMessages;

namespace XpInc.Core.Domain.IntegrantionModels
{
    public class UpdateQuantidadeDisponivelProdutoIntegrationEvent : IntegrationEvent
    {
        public Guid IdProduto { get; set; }
        public int QuantidadeCotas { get; set; }
        public string Nome { get; set; }
        public decimal ValorUnitario { get; set; }
        public bool EhCompra {  get; set; }
        public UpdateQuantidadeDisponivelProdutoIntegrationEvent(Guid idProduto, int quantidadeCotas, string nome, decimal valorUnitario, bool ehCompra)
        {
            IdProduto = idProduto;
            QuantidadeCotas = quantidadeCotas;
            Nome = nome;
            ValorUnitario = valorUnitario;
            EhCompra = ehCompra;

        }
    }
}
