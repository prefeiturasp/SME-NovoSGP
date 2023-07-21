using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace SME.SGP.Aplicacao
{
    public class ExcluirFrequenciaPorAulaIdUseCase : AbstractUseCase, IExcluirFrequenciaPorAulaIdUseCase
    {
        public ExcluirFrequenciaPorAulaIdUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroIdDto>();

            await mediator.Send(new ExcluirFrequenciaDaAulaCommand(filtro.Id));

            var aula = await mediator.Send(new ObterAulaPorIdQuery(filtro.Id));
            
            var turmaId = await mediator.Send(new ObterTurmaIdPorCodigoQuery(aula.TurmaId));
            
            await mediator.Send(new IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand(turmaId, aula.DataAula));
            //Fazer chamada para atualização semanal e mensal - D2
            
            return true;
        }
    }
}
