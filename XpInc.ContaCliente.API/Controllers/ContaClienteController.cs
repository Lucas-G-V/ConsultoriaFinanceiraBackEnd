using Microsoft.AspNetCore.Mvc;
using XpInc.ApiConfig.Controllers;
using XpInc.ApiConfig.Services;
using XpInc.Cache;

namespace XpInc.ContaCliente.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContaClienteController : MainController
    {
        private readonly IMemoryCacheService _cache;
        private readonly IUsuarioService _usuarioService;
        public ContaClienteController(IMemoryCacheService cache, IUsuarioService usuarioService)
        {
            _cache = cache;
            _usuarioService = usuarioService;
        }

        [HttpGet("GetMemory")]
        [ClaimsAuthorize("ContaCliente", "Escrever")]
        public async Task<IActionResult>  GetMemory()
        {
            var user = new User("Cleber Machado");
            await _cache.AddMemoryCache<User>("1", user);
            var userResponse = await _cache.GetById<User>("1");
            var userId = _usuarioService.GetUserId();

            return Ok(userId);
        }
    }
    public class User
    {
        public string Nome { get; set; }
        public User(string nome)
        {
            Nome = nome;
        }
    }
}
