using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace SME.SGP.Aplicacao
{
    public class AlterarAulaFrequenciaTratarUseCase : AbstractUseCase, IAlterarAulaFrequenciaTratarUseCase
    {
        public AlterarAulaFrequenciaTratarUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<AulaAlterarFrequenciaRequestDto>();

            var aulaParaTratar = await mediator.Send(new ObterAulaPorIdQuery(filtro.AulaId));
            if (aulaParaTratar != null)
            {
                await mediator.Send(new AlterarAulaFrequenciaTratarCommand(aulaParaTratar, filtro.QuantidadeAnterior));
                await mediator.Send(new RecalcularFrequenciaPorTurmaCommand(aulaParaTratar.TurmaId, aulaParaTratar.DisciplinaId, aulaParaTratar.Id));
                var turmaId = await mediator.Send(new ObterTurmaIdPorCodigoQuery(aulaParaTratar.TurmaId));
                await mediator.Send(new IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand(turmaId, aulaParaTratar.DataAula));
                //Fazer chamada para atualização semanal e mensal - D2
            }

            return true;
        }
    }
}
