using XpInc.Core.Messages;

namespace XpInc.ContaCliente.API.Application.Commands
{
    public class AlteraSaldoClienteCommand : Command
    {
        public Guid IdCliente { get; set; }
        public decimal SaldoDisponivel { get; set; }
        public decimal TotalInvestido { get; set; }
        public AlteraSaldoClienteCommand(Guid idCliente, decimal saldoDisponivel, decimal totalInvestido)
        {
            SaldoDisponivel = saldoDisponivel;
            IdCliente = idCliente;
            TotalInvestido = totalInvestido;    
        }
    }
}
