using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronizacaoConsolidacaoMatriculasTurmasUseCase : AbstractUseCase, IExecutarSincronizacaoConsolidacaoMatriculasTurmasUseCase
    {
        public ExecutarSincronizacaoConsolidacaoMatriculasTurmasUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var matricula = mensagemRabbit.ObterObjetoMensagem<ConsolidacaoMatriculaTurmaDto>();

            await mediator.Send(new RegistraConsolidacaoMatriculaTurmaCommand(matricula.TurmaId, matricula.Quantidade));
            return true;
        }
    }
}
