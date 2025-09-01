using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;

namespace Test.Mocks;

public class VeiculoServicoMock : IVeiculoServico
{
    private static List<Veiculo> veiculos = new List<Veiculo>(){
        new Veiculo{
            Id = 1,
            Nome = "Fusca",
            Marca = "Volkswagen",
            Ano = 1980
        }
    };

    public void Apagar(Veiculo veiculo)
    {
        veiculos.RemoveAll(v => v.Id == veiculo.Id);
    }

    public void Atualizar(Veiculo veiculo)
    {
        var v = veiculos.Find(x => x.Id == veiculo.Id);
        if (v != null)
        {
            v.Nome = veiculo.Nome;
            v.Marca = veiculo.Marca;
            v.Ano = veiculo.Ano;
        }
    }

    public Veiculo? BuscaPorId(int id)
    {
        return veiculos.Find(v => v.Id == id);
    }

    public void Incluir(Veiculo veiculo)
    {
        veiculo.Id = veiculos.Count + 1;
        veiculos.Add(veiculo);
    }

    public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
    {
        return veiculos;
    }
}
