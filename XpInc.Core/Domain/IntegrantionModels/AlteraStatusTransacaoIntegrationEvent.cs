using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpInc.Core.Messages.IntegrationMessages;

namespace XpInc.Core.Domain.IntegrantionModels
{
    public class AlteraStatusTransacaoIntegrationEvent : IntegrationEvent
    {
        public bool Aprovada { get; set; }
    }
}
