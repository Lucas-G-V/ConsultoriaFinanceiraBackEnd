using XpInc.Core.Messages;
using XpInc.Transacao.API.Models.Enums;

namespace XpInc.Transacao.API.Application.Commands
{
    public class AlteraStatusTransacaoCommand : Command
    {
        public Guid Id { get; set; }
        public StatusTransacao Status { get; set; }
        public AlteraStatusTransacaoCommand(StatusTransacao statusTransacao, Guid id)
        {
            Id = id;
            Status = statusTransacao;
        }
    }
}
