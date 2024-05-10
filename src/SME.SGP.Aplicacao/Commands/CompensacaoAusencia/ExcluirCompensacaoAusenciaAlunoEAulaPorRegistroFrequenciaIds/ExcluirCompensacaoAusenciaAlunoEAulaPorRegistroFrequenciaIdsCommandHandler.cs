using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCompensacaoAusenciaAlunoEAulaPorRegistroFrequenciaIdsCommandHandler : IRequestHandler<ExcluirCompensacaoAusenciaAlunoEAulaPorRegistroFrequenciaIdsCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioCompensacaoAusenciaAlunoAulaConsulta repositorioCompensacaoAusenciaAlunoAulaConsulta;

        public ExcluirCompensacaoAusenciaAlunoEAulaPorRegistroFrequenciaIdsCommandHandler(IMediator mediator, IRepositorioCompensacaoAusenciaAlunoAulaConsulta repositorioCompensacaoAusenciaAlunoAulaConsulta)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioCompensacaoAusenciaAlunoAulaConsulta = repositorioCompensacaoAusenciaAlunoAulaConsulta ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAlunoAulaConsulta));
        }

        public async Task<bool> Handle(ExcluirCompensacaoAusenciaAlunoEAulaPorRegistroFrequenciaIdsCommand request, CancellationToken cancellationToken)
        {
            var compensacaoAusenciaAlunoAulas = await repositorioCompensacaoAusenciaAlunoAulaConsulta.ObterPorRegistroFrequenciaAlunoIdsAsync(request.RegistroFrequenciaAlunoIds.ToArray());

            if(compensacaoAusenciaAlunoAulas.Any())
                await mediator.Send(new ExcluirCompensacaoAusenciaEAlunoEAulaCommand(compensacaoAusenciaAlunoAulas), cancellationToken);

            return true;
        }
    }
}
