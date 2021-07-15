using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDescricoesRegistrosIndividuaisPorPeriodoQueryHandler : IRequestHandler<ObterDescricoesRegistrosIndividuaisPorPeriodoQuery, IEnumerable<RegistroIndividualResumoDto>>
    {
        private readonly IRepositorioRegistroIndividual repositorioRegistroIndividual;

        public ObterDescricoesRegistrosIndividuaisPorPeriodoQueryHandler(IRepositorioRegistroIndividual repositorioRegistroIndividual)
        {
            this.repositorioRegistroIndividual = repositorioRegistroIndividual ?? throw new ArgumentNullException(nameof(repositorioRegistroIndividual));
        }

        public async Task<IEnumerable<RegistroIndividualResumoDto>> Handle(ObterDescricoesRegistrosIndividuaisPorPeriodoQuery request, CancellationToken cancellationToken)
            => await repositorioRegistroIndividual.ObterDescricaoRegistrosIndividuaisPorPeriodo(request.TurmaId, request.ComponenteCurricularId, request.AlunoCodigo, request.DataInicio, request.DataFim);
    }
}
