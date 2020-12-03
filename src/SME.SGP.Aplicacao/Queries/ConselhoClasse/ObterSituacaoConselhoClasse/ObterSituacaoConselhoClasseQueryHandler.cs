using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSituacaoConselhoClasseQueryHandler : IRequestHandler<ObterSituacaoConselhoClasseQuery, SituacaoConselhoClasse>
    {
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;

        public ObterSituacaoConselhoClasseQueryHandler(IRepositorioConselhoClasse repositorioConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
        }

        public async Task<SituacaoConselhoClasse> Handle(ObterSituacaoConselhoClasseQuery request, CancellationToken cancellationToken)
            => await repositorioConselhoClasse.ObterSituacaoConselhoClasse(request.TurmaId, request.PeriodoEscolarId);
    }
}
