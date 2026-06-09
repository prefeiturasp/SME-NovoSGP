using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirWfAprovacaoParecerConclusivoCommandHandler : IRequestHandler<ExcluirWfAprovacaoParecerConclusivoCommand>
    {
        private readonly IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacaoParecerConclusivo;

        public ExcluirWfAprovacaoParecerConclusivoCommandHandler(IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacaoParecerConclusivo)
        {
            this.repositorioWFAprovacaoParecerConclusivo = repositorioWFAprovacaoParecerConclusivo ?? throw new ArgumentNullException(nameof(repositorioWFAprovacaoParecerConclusivo));
        }

        public async Task Handle(ExcluirWfAprovacaoParecerConclusivoCommand request, CancellationToken cancellationToken)
            => await repositorioWFAprovacaoParecerConclusivo.ExcluirLogico(request.Id);
    }
}
