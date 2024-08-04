namespace XpInc.Autenticacao.API.Models.DTO
{
    public class CreateClienteDTO
    {
        public string IdUsuario { get; set; }
        public string NomeCompleto { get; set; }
        public string TelefoneCelular { get; set; }
        public string CPF { get; set; }
    }
}
