using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IExecutarSincronizacaoInstitucionalCicloTratarUseCase
    {
        Task<bool> Executar(MensagemRabbit param);
    }
}