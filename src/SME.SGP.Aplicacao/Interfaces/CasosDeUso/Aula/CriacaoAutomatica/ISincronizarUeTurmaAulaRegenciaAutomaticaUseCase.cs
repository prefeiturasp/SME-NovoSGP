using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ISincronizarUeTurmaAulaRegenciaAutomaticaUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}