using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface INotificarExclusaoAulaComFrequenciaUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}