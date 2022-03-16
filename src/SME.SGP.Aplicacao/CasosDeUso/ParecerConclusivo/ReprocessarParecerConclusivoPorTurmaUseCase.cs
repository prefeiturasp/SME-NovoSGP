using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReprocessarParecerConclusivoPorTurmaUseCase : AbstractUseCase, IReprocessarParecerConclusivoPorTurmaUseCase
    {
        public ReprocessarParecerConclusivoPorTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroDreUeTurmaDto>();
            var registros = await mediator.Send(new ObterConselhoClasseAlunosPorTurmaQuery(filtro.TurmaCodigo));

            foreach (var registro in registros)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaAtualizarParecerConclusivoAluno
                    , registro, Guid.NewGuid(), null));

            return true;
        }
    }
}
