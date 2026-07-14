using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IMoverServicoArmazenamentoUseCase
    {
        Task<string> Executar(string nomeArquivo);
    }
}
