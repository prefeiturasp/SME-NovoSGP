using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Commands
{
    public class GerarParecerConclusivoPorConselhoFechamentoAlunoCommandHandler : IRequestHandler<GerarParecerConclusivoPorConselhoFechamentoAlunoCommand, ParecerConclusivoDto>
    {
        private readonly IMediator mediator;

        public GerarParecerConclusivoPorConselhoFechamentoAlunoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ParecerConclusivoDto> Handle(GerarParecerConclusivoPorConselhoFechamentoAlunoCommand request,CancellationToken cancellationToken)
        {
            var solicitanteId = await mediator.Send(ObterUsuarioLogadoIdQuery.Instance);
            
            var conselhoClasseAluno = await mediator.Send(new  ObterConselhoClasseAlunoPorConselhoFechamentoAlunoCodigoQuery(
                request.ConselhoClasseId, 
                request.FechamentoTurmaId, 
                request.AlunoCodigo));

            return await mediator.Send(new GerarParecerConclusivoAlunoCommand(conselhoClasseAluno, solicitanteId));
        }
    }
}