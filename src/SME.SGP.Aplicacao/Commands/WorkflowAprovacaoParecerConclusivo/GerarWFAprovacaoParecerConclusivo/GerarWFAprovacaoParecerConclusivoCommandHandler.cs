using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarWFAprovacaoParecerConclusivoCommandHandler : AsyncRequestHandler<GerarWFAprovacaoParecerConclusivoCommand>
    {
        private readonly IRepositorioWFAprovacaoParecerConclusivo repositorio;
        private readonly IMediator mediator;

        public GerarWFAprovacaoParecerConclusivoCommandHandler(IRepositorioWFAprovacaoParecerConclusivo repositorio, IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(GerarWFAprovacaoParecerConclusivoCommand request, CancellationToken cancellationToken)
        {
            await ExcluirWorkflow(request.ConselhoClasseAlunoId);

            await repositorio.SalvarAsync(new WFAprovacaoParecerConclusivo()
            {
                ConselhoClasseAlunoId = request.ConselhoClasseAlunoId,
                ConselhoClasseParecerId = request.ParecerConclusivoId,
                UsuarioSolicitanteId = request.UsuarioSolicitanteId,
                ConselhoClasseParecerAnteriorId = request.ParecerConclusivoAnteriorId
            });
        }

        private async Task ExcluirWorkflow(long conselhoClasseAlunoId)
        {
            await mediator.Send(new ExcluirWFAprovacaoParecerPorAlunoCommand(conselhoClasseAlunoId));
        }
    }
}
