﻿using XpInc.RendaFixa.API.Models.Enum;

namespace XpInc.RendaFixa.API.Models.DTO.Request
{
    public class UpdateRendaFixaRequest
    {
        public Guid Id { get; set; }
        public int? QuantidadeCotasDisponivel { get; set; }
        public string Nome { get; set; }
        public decimal ValorMinimo { get; set; }
        public decimal ValorUnitario { get; set; }
        public string EmailAdministrador { get; set; }
    }
}
