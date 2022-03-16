using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaConsolidacaoSemanalDashBoardFrequenciaUseCase : AbstractUseCase, IExecutaConsolidacaoSemanalDashBoardFrequenciaUseCase
    {
        public ExecutaConsolidacaoSemanalDashBoardFrequenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FiltroConsolicacaoGeralDashBoardFrequenciaDto>();

            if (filtro == null)
            {
                var mensagem = $"Não foi possível iniciar a consolidação diária";
                throw new NegocioException(mensagem);
            }

            var uesCodigos = await mediator.Send(new ObterCodigosUEsQuery());

            if (uesCodigos == null || !uesCodigos.Any())
                return false;

            foreach (var ueCodigo in uesCodigos)
            {
                var turmasIds = await mediator.Send(new ObterTurmasIdPorUeCodigoEAnoLetivoQuery(filtro.AnoLetivo, ueCodigo));

                if (turmasIds == null || !turmasIds.Any())
                    continue;

                foreach (var turmaId in turmasIds)
                    await mediator.Send(new InserirConsolidacaoSemanalDashBoardFrequenciaCommand(filtro.AnoLetivo, filtro.Mes, turmaId));
            }

            return true;
        }
    }
}
