using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaConsolidacaoDiariaDashBoardFrequenciaPorUeUseCase : AbstractUseCase, IExecutaConsolidacaoDiariaDashBoardFrequenciaPorUeUseCase
    {
        public ExecutaConsolidacaoDiariaDashBoardFrequenciaPorUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<ConsolidacaoPorUeDashBoardFrequencia>();

            var turmasIds = await mediator.Send(new ObterTurmasIdPorUeCodigoEAnoLetivoQuery(filtro.AnoLetivo, filtro.UeCodigo));

            foreach (var turmaId in turmasIds)
            {
                var dados = new ConsolidacaoPorTurmaDashBoardFrequencia()
                {
                    AnoLetivo = filtro.AnoLetivo,
                    Mes = filtro.Mes,
                    TurmaId = turmaId
                };
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConsolidacaoDiariaDashBoardFrequenciaPorTurma, dados, Guid.NewGuid(), null));
            }

            return true;
        }
    }
}
