using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
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
                throw new NegocioException(mensagem);
            }
            
            var uesCodigos = await mediator.Send(new ObterCodigosUEsQuery());

            if (uesCodigos == null || !uesCodigos.Any())
                return false;

            foreach (var ueCodigo in uesCodigos)
            {
                var dados = new ConsolidacaoPorUeDashBoardFrequencia()
                {
                    AnoLetivo = filtro.AnoLetivo,
                    Mes = filtro.Mes,
                    UeCodigo = ueCodigo,
                    TipoPeriodo = TipoPeriodoDashboardFrequencia.Diario
                };

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConsolidacaoDashBoardFrequenciaPorUe, dados, Guid.NewGuid(), null));
            }

            return true;
        }
    }
}
