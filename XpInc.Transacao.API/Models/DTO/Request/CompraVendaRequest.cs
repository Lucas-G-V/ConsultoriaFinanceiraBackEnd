using System.ComponentModel.DataAnnotations;

namespace XpInc.Transacao.API.Models.DTO.Request
{
    public class CompraVendaRequest
    {
        [Required]
        public Guid ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
    }
}
