using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSituacaoConselhoClasseQueryHandler : IRequestHandler<ObterSituacaoConselhoClasseQuery, SituacaoConselhoClasse>
    {
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasseConsulta;

        public ObterSituacaoConselhoClasseQueryHandler(IRepositorioConselhoClasse repositorioConselhoClasse)
        {
            this.repositorioConselhoClasseConsulta = repositorioConselhoClasseConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseConsulta));
        }

        public async Task<SituacaoConselhoClasse> Handle(ObterSituacaoConselhoClasseQuery request, CancellationToken cancellationToken)
            => await repositorioConselhoClasseConsulta.ObterSituacaoConselhoClasse(request.TurmaId, request.PeriodoEscolarId);
    }
}
