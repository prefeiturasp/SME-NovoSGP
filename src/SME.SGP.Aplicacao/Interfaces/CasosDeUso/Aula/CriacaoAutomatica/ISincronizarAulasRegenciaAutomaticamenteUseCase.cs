using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ISincronizarAulasRegenciaAutomaticamenteUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
