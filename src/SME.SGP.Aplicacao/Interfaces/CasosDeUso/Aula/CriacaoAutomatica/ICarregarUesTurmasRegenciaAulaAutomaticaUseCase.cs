using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ICarregarUesTurmasRegenciaAulaAutomaticaUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
