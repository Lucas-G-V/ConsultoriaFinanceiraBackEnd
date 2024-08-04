using XpInc.Core.Messages;

namespace XpInc.RendaFixa.API.Application.Commands
{
    public class ProcessoCompraRendaFixaCommand : Command
    {
        public Guid Id { get; set; }
        public int QuantidadeDeCotasDebitadas { get; set; }
        public string Nome { get; set; }
        public decimal ValorUnitario { get; set; }
        public ProcessoCompraRendaFixaCommand(Guid idProduto, int quantidadeDeCotasDebitadas, string nome, decimal valorUnitario)
        {
            Id = idProduto;
            QuantidadeDeCotasDebitadas = quantidadeDeCotasDebitadas;
            Nome = nome;
            ValorUnitario = valorUnitario;
        }
    }
}
