using XpInc.Core.Messages;
using XpInc.RendaFixa.API.Models.Enum;

namespace XpInc.RendaFixa.API.Application.Commands
{
    public class UpdateRendaFixaCommand : Command
    {
        public Guid Id { get; set; }
        public int? QuantidadeCotasDisponivel { get; set; }
        public int? QuantidadeDeCotasDebitadas { get; set; }
        public UpdateRendaFixaCommand(Guid idProduto, int? quantidadeCotaDisponivel = null, int? quantidadeDeCotasDebitadas = null)
        {
            Id = idProduto;
            QuantidadeCotasDisponivel = quantidadeCotaDisponivel;
            QuantidadeDeCotasDebitadas = quantidadeDeCotasDebitadas;
        }
    }
}
