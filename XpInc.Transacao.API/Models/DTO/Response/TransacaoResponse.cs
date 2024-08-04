﻿using XpInc.Transacao.API.Models.Enums;

namespace XpInc.Transacao.API.Models.DTO.Response
{
    public class TransacaoResponse
    {
        public Guid? ProdutoId { get; set; }
        public string? NomeProduto { get; set; }
        public TipoTransacao Tipo { get; set; }
        public StatusTransacao Status { get; set; }
        public DateTime DataTransacao { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
