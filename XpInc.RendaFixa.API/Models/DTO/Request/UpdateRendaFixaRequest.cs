using System.ComponentModel.DataAnnotations;
using XpInc.RendaFixa.API.Models.Enum;

namespace XpInc.RendaFixa.API.Models.DTO.Request
{
    public class UpdateRendaFixaRequest
    {
        public Guid Id { get; set; }
        public int? QuantidadeCotasDisponivel { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O valor mínimo é obrigatório.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "O valor mínimo deve ser maior que 0.")]
        public decimal ValorMinimo { get; set; }
        [Required(ErrorMessage = "O valor mínimo é obrigatório.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "O valor mínimo deve ser maior que 0.")]
        public decimal ValorUnitario { get; set; }
        [Required(ErrorMessage = "O email é obrigatório.")]
        public string EmailAdministrador { get; set; }
    }
}
