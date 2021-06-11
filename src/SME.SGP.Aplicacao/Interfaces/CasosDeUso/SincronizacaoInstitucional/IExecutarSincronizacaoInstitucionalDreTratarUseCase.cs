using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IExecutarSincronizacaoInstitucionalDreTratarUseCase
    {
        Task<bool> Executar(MensagemRabbit param);
    }
}
