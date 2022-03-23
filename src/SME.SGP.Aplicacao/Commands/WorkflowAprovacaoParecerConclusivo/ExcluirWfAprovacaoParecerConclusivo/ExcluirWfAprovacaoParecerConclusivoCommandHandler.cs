using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirWfAprovacaoParecerConclusivoCommandHandler : AsyncRequestHandler<ExcluirWfAprovacaoParecerConclusivoCommand>
    {
        private readonly IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacaoParecerConclusivo;

        public ExcluirWfAprovacaoParecerConclusivoCommandHandler(IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacaoParecerConclusivo)
        {
            this.repositorioWFAprovacaoParecerConclusivo = repositorioWFAprovacaoParecerConclusivo ?? throw new ArgumentNullException(nameof(repositorioWFAprovacaoParecerConclusivo));
        }

        protected override async Task Handle(ExcluirWfAprovacaoParecerConclusivoCommand request, CancellationToken cancellationToken)
            => await repositorioWFAprovacaoParecerConclusivo.Excluir(request.Id);
    }
}
