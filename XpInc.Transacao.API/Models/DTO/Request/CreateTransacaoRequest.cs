using XpInc.Transacao.API.Models.Enums;

namespace XpInc.Transacao.API.Models.DTO.Request
{
    public class CreateTransacaoRequest
    {
        public Guid? ProdutoId { get; set; }
        public string? NomeProduto { get; set; }
        public TipoTransacao Tipo { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
