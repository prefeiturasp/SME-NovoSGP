using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IExcluirTemporarioServicoArmazenamentoUseCase
    {
        Task<bool> Executar(string nomeArquivo, string bucketTemporario);
    }
}
