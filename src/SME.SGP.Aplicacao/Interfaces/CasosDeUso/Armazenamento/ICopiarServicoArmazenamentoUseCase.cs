using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ICopiarServicoArmazenamentoUseCase
    {
        Task<string> Executar(string nomeArquivo);
    }
}
