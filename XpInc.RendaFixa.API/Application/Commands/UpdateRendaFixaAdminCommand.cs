using XpInc.Core.Messages;

namespace XpInc.RendaFixa.API.Application.Commands
{
    public class UpdateRendaFixaAdminCommand : Command
    {
        public Guid Id { get; set; }
        public int? QuantidadeCotasDisponivel { get; set; }
        public decimal ValorMinimo { get; set; }
        public decimal ValorUnitario { get; set; }
        public string EmailAdministrador { get; set; }
    }
}
