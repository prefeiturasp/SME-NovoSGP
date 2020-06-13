using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.ParecerConclusivo.ObterPareceresConclusivosPorTurma
{
    public class ObterPareceresConclusivosPorTurmaQueryHandler : IRequestHandler<ObterPareceresConclusivosPorTurmaQuery, IEnumerable<ParecerConclusivoDto>>
    {
        private readonly IRepositorioConselhoClasseParecerConclusivo repositorioConselhoClasseParecerConclusivo;

        public ObterPareceresConclusivosPorTurmaQueryHandler(IRepositorioConselhoClasseParecerConclusivo repositorioConselhoClasseParecerConclusivo)
        {
            this.repositorioConselhoClasseParecerConclusivo = repositorioConselhoClasseParecerConclusivo ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseParecerConclusivo));
        }

        public async Task<IEnumerable<ParecerConclusivoDto>> Handle(ObterPareceresConclusivosPorTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioConselhoClasseParecerConclusivo.ObterListaResumidaPorTurmaCodigoAsync(request.TurmaCodigo, request.DataConsulta);
    }
}
