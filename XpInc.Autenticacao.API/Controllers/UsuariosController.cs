using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using XpInc.ApiConfig.Config;
using XpInc.ApiConfig.Controllers;
using XpInc.Autenticacao.API.Models.Request;
using XpInc.Bus;
using XpInc.Core.Domain.IntegrantionModels;
using XpInc.Core.Messages.IntegrationMessages;

namespace XpInc.Autenticacao.API.Controllers
{
    public class UsuariosController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationSettings _appSettings;
        private readonly IMessageBus _bus;
        public UsuariosController(SignInManager<IdentityUser> signInManager,
                             UserManager<IdentityUser> userManager,
                             IOptions<ApplicationSettings> appSettings,
                             IMessageBus bus)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _bus = bus;
        }


        [HttpPost("ContaAdmin")]
        public async Task<ActionResult> Registrar(CreateUserAdminRequest usuarioRegistro)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Login,
                Email = usuarioRegistro.Login,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, usuarioRegistro.Senha);

            if (result.Succeeded)
            {
                await AddRolesAdmin(user);
                await AddClaimsAdmin(user);
                return NoContent();
            }

            foreach (var error in result.Errors)
            {
                AdicionarErroProcessamento(error.Description);
            }

            return CustomResponse();
        }

        private async Task AddRolesAdmin(IdentityUser adminUser)
        {
            await _userManager.AddToRoleAsync(adminUser, "ContaCliente");
            await _userManager.AddToRoleAsync(adminUser, "Transacao");
            await _userManager.AddToRoleAsync(adminUser, "RendaFixa");
        }

        private async Task AddClaimsAdmin(IdentityUser adminUser)
        {
            await _userManager.AddClaimAsync(adminUser, new Claim("Transacao", "LerRestrito"));
            await _userManager.AddClaimAsync(adminUser, new Claim("RendaFixa", "LerRestrito"));
            await _userManager.AddClaimAsync(adminUser, new Claim("RendaFixa", "Escrever"));
            await _userManager.AddClaimAsync(adminUser, new Claim("RendaFixa", "Editar"));
            await _userManager.AddClaimAsync(adminUser, new Claim("ContaCliente", "LerRestrito"));
        }

        [HttpPost("ContaCliente")]
        public async Task<ActionResult> RegistrarCliente(CreateUserClienteRequest usuarioRegistro)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Login,
                Email = usuarioRegistro.Login,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, usuarioRegistro.Senha);

            if (result.Succeeded)
            {
                await AddRolesCliente(user);
                await AddClaimsCliente(user);

                var resultCliente = await RegistrarClienteModel(usuarioRegistro);
                if (resultCliente.ValidationResult.IsValid) return NoContent();
                return CustomResponse(resultCliente.ValidationResult);

            }

            foreach (var error in result.Errors)
            {
                AdicionarErroProcessamento(error.Description);
            }

            return CustomResponse();
        }

        private async Task AddRolesCliente(IdentityUser adminUser)
        {
            await _userManager.AddToRoleAsync(adminUser, "ContaCliente");
            await _userManager.AddToRoleAsync(adminUser, "Transacao");
            await _userManager.AddToRoleAsync(adminUser, "RendaFixa");
        }

        private async Task AddClaimsCliente(IdentityUser adminUser)
        {
            await _userManager.AddClaimAsync(adminUser, new Claim("Transacao", "Ler"));
            await _userManager.AddClaimAsync(adminUser, new Claim("Transacao", "Escrever"));

            await _userManager.AddClaimAsync(adminUser, new Claim("RendaFixa", "Ler"));

            await _userManager.AddClaimAsync(adminUser, new Claim("ContaCliente", "Ler"));
            await _userManager.AddClaimAsync(adminUser, new Claim("ContaCliente", "Escrever"));
            await _userManager.AddClaimAsync(adminUser, new Claim("ContaCliente", "Editar"));
        }

        private async Task<ResponseMessage> RegistrarClienteModel(CreateUserClienteRequest clienteNovo)
        {
            var usuario = await _userManager.FindByEmailAsync(clienteNovo.Login);

            var clienteRegistrado = new CreateClienteIntegrationEvent(Guid.Parse(usuario.Id), 
                clienteNovo.NomeCompleto, clienteNovo.CPF, clienteNovo.TelefoneCelular);

            try
            {
                var result = await _bus.RequestAsync<CreateClienteIntegrationEvent, ResponseMessage>(clienteRegistrado);
                if(!result.ValidationResult.IsValid) await _userManager.DeleteAsync(usuario);
                return result;
            }
            catch
            {
                await _userManager.DeleteAsync(usuario);
                throw;
            }
        }
    }
}
