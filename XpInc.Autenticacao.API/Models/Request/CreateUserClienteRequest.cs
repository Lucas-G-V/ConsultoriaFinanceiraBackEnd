namespace XpInc.Autenticacao.API.Models.Request
{
    public class CreateUserClienteRequest
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public string NomeCompleto { get; set; }
        public string CPF { get; set; }
        public string TelefoneCelular { get; set; }
    }
}
