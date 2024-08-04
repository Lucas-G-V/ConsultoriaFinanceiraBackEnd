using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpInc.Core.Messages.IntegrationMessages;

namespace XpInc.Core.Domain.IntegrantionModels
{
    public class AlteraSaldoClienteIntegrantionEvent : IntegrationEvent
    {
        public Guid IdCliente { get; set; }
        public decimal ValorInvestido { get; set; }
        public decimal SaldoConta { get; set; }
        public AlteraSaldoClienteIntegrantionEvent(Guid idCliente, decimal valorInvestido, decimal saldoConta)
        {
            IdCliente = idCliente;
            ValorInvestido = valorInvestido;
            SaldoConta = saldoConta;
        }
        public AlteraSaldoClienteIntegrantionEvent()
        {
            
        }
    }
}
