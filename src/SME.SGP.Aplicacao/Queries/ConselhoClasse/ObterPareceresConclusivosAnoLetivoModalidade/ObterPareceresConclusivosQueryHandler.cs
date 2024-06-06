using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPareceresConclusivosAnoLetivoModalidadeQueryHandler : IRequestHandler<ObterPareceresConclusivosAnoLetivoModalidadeQuery, IEnumerable<ParecerConclusivoDto>>
    {
        private readonly IRepositorioConselhoClasseParecerConclusivo repositorioConselhoClasseParecerConclusivo;

        public ObterPareceresConclusivosAnoLetivoModalidadeQueryHandler(IRepositorioConselhoClasseParecerConclusivo repositorioConselhoClasseParecerConclusivo)
        {
            this.repositorioConselhoClasseParecerConclusivo = repositorioConselhoClasseParecerConclusivo ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseParecerConclusivo));
        }

        public async Task<IEnumerable<ParecerConclusivoDto>> Handle(ObterPareceresConclusivosAnoLetivoModalidadeQuery request, CancellationToken cancellationToken)
        {
            var pareceresConclusivos = await repositorioConselhoClasseParecerConclusivo.ObterPareceresConclusivos(request.AnoLetivo, request.Modalidade);
            return pareceresConclusivos.OrderBy(p => p.Id);
        }
    }
}
