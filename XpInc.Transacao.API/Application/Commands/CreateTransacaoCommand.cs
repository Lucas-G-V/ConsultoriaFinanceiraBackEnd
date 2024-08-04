using XpInc.Core.Messages;
using XpInc.Transacao.API.Models.Enums;

namespace XpInc.Transacao.API.Application.Commands
{
    public class CreateTransacaoCommand : Command
    {
        public Guid ClienteId { get; set; }
        public Guid? ProdutoId { get; set; }
        public string? NomeProduto { get; set; }
        public TipoTransacao Tipo { get; set; }
        public StatusTransacao Status { get; set; } = StatusTransacao.Pendente;
        public DateTime DataTransacao { get; set; } = DateTime.Now;
        public decimal? Quantidade { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
