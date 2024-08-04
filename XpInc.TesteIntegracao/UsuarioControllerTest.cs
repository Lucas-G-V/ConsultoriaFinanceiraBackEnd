using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using XpInc.TesteIntegracao;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using XpInc.Autenticacao.API.Models.Request;


public class CustomWebApplicationFactoryFixture
{
    public CustomWebApplicationFactory Factory { get; }

    public CustomWebApplicationFactoryFixture()
    {
        Factory = new CustomWebApplicationFactory();
    }
}

[CollectionDefinition("CustomWebApplicationFactory")]
public class CustomWebApplicationFactoryCollection : ICollectionFixture<CustomWebApplicationFactoryFixture>
{
}

public class UsuarioTests : IClassFixture<CustomWebApplicationFactoryFixture>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public UsuarioTests(CustomWebApplicationFactoryFixture fixture)
    {
        _factory = fixture.Factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Registrar_Usuario_Deve_Funcionar()
    {
        var request = new CreateUserAdminRequest
        {
            Login = "test@example.com",
            Senha = "Password123!"
        };

        var response = await _client.PostAsJsonAsync("/api/Usuarios/ContaAdmin", request);

        response.EnsureSuccessStatusCode();
    }
}
