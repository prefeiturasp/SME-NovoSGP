using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IExecutarSincronizacaoInstitucionalCicloUseCase
    {
        Task<bool> Executar(MensagemRabbit param);
    }
}
