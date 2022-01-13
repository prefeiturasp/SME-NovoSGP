using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase : AbstractUseCase, IExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase
    {
        public ExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {            
            var filtro = mensagemRabbit.ObterObjetoMensagem<FiltroConsolicacaoGeralDashBoardFrequenciaDto>();

            if (filtro == null)
            {
                var mensagem = $"Não foi possível iniciar a consolidação diária";
                SentrySdk.CaptureMessage(mensagem);
                throw new NegocioException(mensagem);
            }
            
            var uesCodigos = await mediator.Send(new ObterCodigosUEsQuery());

            if (uesCodigos == null || !uesCodigos.Any())
                return false;

            foreach(var ueCodigo in uesCodigos)
            {                
                var turmasIds = await mediator.Send(new ObterTurmasIdPorUeCodigoEAnoLetivoQuery(filtro.AnoLetivo, ueCodigo));

                if (turmasIds == null || !turmasIds.Any())
                    continue;

                foreach (var turmaId in turmasIds)                
                    await mediator.Send(new InserirConsolidacaoDiariaDashBoardFrequenciaCommand(filtro.AnoLetivo, filtro.Mes, turmaId));
            }

            return true;
        }
    }
}
