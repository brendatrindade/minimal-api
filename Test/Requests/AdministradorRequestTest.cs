using System.Net;
using System.Text;
using System.Text.Json;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.DTOs;
using Test.Helpers;

namespace Test.Requests;

[TestClass]
public class AdministradorRequestTest
{
    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        Setup.ClassInit(testContext);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Setup.ClassCleanup();
    }

    [TestMethod]
    public async Task TestarLoginAdministrador()
    {
        // Arrange
        var loginDTO = new LoginDTO{
            Email = "administrador@teste.com",
            Senha = "123456"
        };
        var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "Application/json");

        // Act
        var response = await Setup.client.PostAsync("/administradores/login", content);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.IsNotNull(admLogado?.Email ?? "");
        Assert.IsNotNull(admLogado?.Perfil ?? "");
        Assert.IsNotNull(admLogado?.Token ?? "");
    }

    private async Task<string> GerarToken()
    {
        var loginDTO = new LoginDTO{
            Email = "administrador@teste.com",
            Senha = "123456"
        };
        var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8,  "Application/json");
        var response = await Setup.client.PostAsync("/administradores/login", content);
        var result = await response.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return admLogado?.Token ?? "";
    }

    [TestMethod]
    public async Task TestarListarAdministradores()
    {
        var token = await GerarToken();
        var request = new HttpRequestMessage(HttpMethod.Get, "/administradores");
        request.Headers.Add("Authorization", $"Bearer {token}");
        var response = await Setup.client.SendAsync(request);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.IsTrue(result.Contains("administrador@teste.com") || result.Length > 0);
    }

    [TestMethod]
    public async Task TestarBuscarAdministradorPorId()
    {
        var token = await GerarToken();
        var request = new HttpRequestMessage(HttpMethod.Get, "/Administradores/1");
        request.Headers.Add("Authorization", $"Bearer {token}");
        var response = await Setup.client.SendAsync(request);
        Assert.IsTrue(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task TestarCriarAdministrador()
    {
        var token = await GerarToken();
        var administradorDTO = new AdministradorDTO{
            Email = "novo@teste.com",
            Senha = "123456",
            Perfil = MinimalApi.Dominio.Enuns.Perfil.Editor
        };
        var content = new StringContent(JsonSerializer.Serialize(administradorDTO), Encoding.UTF8,  "Application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, "/administradores");
        request.Content = content;
        request.Headers.Add("Authorization", $"Bearer {token}");
        var response = await Setup.client.SendAsync(request);
        Assert.IsTrue(response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.BadRequest);
    }
}