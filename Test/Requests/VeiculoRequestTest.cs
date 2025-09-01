using System.Net;
using System.Text;
using System.Text.Json;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.DTOs;
using Test.Helpers;

namespace Test.Requests;

[TestClass]
public class VeiculoRequestTest
{
    private static string? token;

    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        Setup.ClassInit(testContext);
        token = GerarToken();
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Setup.ClassCleanup();
    }

    private static string GerarToken()
    {
        var loginDTO = new LoginDTO{
            Email = "administrador@teste.com",
            Senha = "123456"
        };
        var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8,  "Application/json");
        var response = Setup.client.PostAsync("/administradores/login", content).Result;
        var result = response.Content.ReadAsStringAsync().Result;
        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return admLogado?.Token ?? "";
    }

    [TestMethod]
    public async Task TestarCriarVeiculo()
    {
        var veiculoDTO = new VeiculoDTO{
            Nome = "Fusca",
            Marca = "Volkswagen",
            Ano = 1980
        };
        var content = new StringContent(JsonSerializer.Serialize(veiculoDTO), Encoding.UTF8,  "Application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, "/veiculos");
        request.Content = content;
        request.Headers.Add("Authorization", $"Bearer {token}");
        var response = await Setup.client.SendAsync(request);
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
    }

    [TestMethod]
    public async Task TestarListarVeiculos()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/veiculos");
        request.Headers.Add("Authorization", $"Bearer {token}");
        var response = await Setup.client.SendAsync(request);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.IsTrue(result.Contains("Fusca") || result.Length > 0);
    }

    [TestMethod]
    public async Task TestarBuscarVeiculoPorId()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/veiculos/1");
        request.Headers.Add("Authorization", $"Bearer {token}");
        var response = await Setup.client.SendAsync(request);
        Assert.IsTrue(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task TestarAtualizarVeiculo()
    {
        var veiculoDTO = new VeiculoDTO{
            Nome = "Fusca Atualizado",
            Marca = "Volkswagen",
            Ano = 1981
        };
        var content = new StringContent(JsonSerializer.Serialize(veiculoDTO), Encoding.UTF8,  "Application/json");
        var request = new HttpRequestMessage(HttpMethod.Put, "/veiculos/1");
        request.Content = content;
        request.Headers.Add("Authorization", $"Bearer {token}");
        var response = await Setup.client.SendAsync(request);
        Assert.IsTrue(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task TestarApagarVeiculo()
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, "/veiculos/1");
        request.Headers.Add("Authorization", $"Bearer {token}");
        var response = await Setup.client.SendAsync(request);
        Assert.IsTrue(response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound);
    }
}