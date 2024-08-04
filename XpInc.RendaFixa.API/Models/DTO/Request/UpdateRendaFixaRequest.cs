using XpInc.RendaFixa.API.Models.Enum;

namespace XpInc.RendaFixa.API.Models.DTO.Request
{
    public class UpdateRendaFixaRequest
    {
        public Guid Id { get; set; }
        public int? QuantidadeCotasDisponivel { get; set; }
    }
}
