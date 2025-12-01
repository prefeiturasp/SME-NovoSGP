using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PendenciaDiarioBordo
{
    public interface IPendenciaDiarioBordoParaExcluirUseCase : IRabbitUseCase 
    {
        Task<bool> Executar(MensagemRabbit param);
    }
}
