using System.ComponentModel.DataAnnotations;
using XpInc.RendaFixa.API.Models.Enum;

namespace XpInc.RendaFixa.API.Models.DTO.Request
{
    public class CreateRendaFixaRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O valor mínimo é obrigatório.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "O valor mínimo deve ser maior que 0.")]
        public decimal ValorMinimo { get; set; }

        [Required(ErrorMessage = "O valor unitário é obrigatório.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "O valor unitário deve ser maior que 0.")]
        public decimal ValorUnitario { get; set; }

        [Required(ErrorMessage = "É necessário indicar se é baseado em cotas.")]
        public bool BaseadoEmCotas { get; set; }

        [Required(ErrorMessage = "A data de vencimento é obrigatória.")]
        public DateTime DataVencimento { get; set; }

        [Required(ErrorMessage = "O tipo de taxa é obrigatório.")]
        public TipoTaxa TipoTaxa { get; set; }

        [Required(ErrorMessage = "A taxa anual é obrigatória.")]
        [Range(0, double.MaxValue, ErrorMessage = "A taxa anual deve ser um valor positivo.")]
        public decimal TaxaAnual { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "A taxa adicional deve ser um valor positivo.")]
        public decimal TaxaAdicional { get; set; }

        [Required(ErrorMessage = "O indexador é obrigatório.")]
        public Indexador Indexador { get; set; }

        [Required(ErrorMessage = "A frequência de pagamento é obrigatória.")]
        public FrequenciaPagamento Frequencia { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "A quantidade inicial de cotas deve ser maior que 0.")]
        public int? QuantidadeCotasInicial { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "A quantidade de cotas disponível deve ser maior que 0.")]
        public int? QuantidadeCotasDisponivel { get; set; }

        [Required(ErrorMessage = "O email do administrador é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email do administrador deve ser um email válido.")]
        public string EmailAdministrador { get; set; }
    }
}
