namespace MinimalApi.Dominio.ModelViews;

public class Home
{
    public string Mensagem { get; } = "API de veículos - Minimal API";
    public string Doc { get; } = "Acesse o Swagger UI adicionando /swagger ao final da rota";
}