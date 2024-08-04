using XpInc.Core.Messages;
using XpInc.RendaFixa.API.Models.Enum;

namespace XpInc.RendaFixa.API.Application.Commands
{
    public class CreateRendaFixaCommand : Command
    {
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
        public string EmailAdministrador { get; set; }
    }
}
