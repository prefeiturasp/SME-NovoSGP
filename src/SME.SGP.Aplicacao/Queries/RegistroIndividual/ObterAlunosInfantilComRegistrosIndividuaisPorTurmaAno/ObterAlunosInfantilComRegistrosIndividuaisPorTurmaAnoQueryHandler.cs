using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosInfantilComRegistrosIndividuaisPorTurmaAnoQueryHandler : IRequestHandler<ObterAlunosInfantilComRegistrosIndividuaisPorTurmaAnoQuery, IEnumerable<AlunoInfantilComRegistroIndividualDTO>>
    {
        private readonly IRepositorioRegistroIndividual repositorio;

        public ObterAlunosInfantilComRegistrosIndividuaisPorTurmaAnoQueryHandler(IRepositorioRegistroIndividual repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<AlunoInfantilComRegistroIndividualDTO>> Handle(ObterAlunosInfantilComRegistrosIndividuaisPorTurmaAnoQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterAlunosInfantilComRegistrosIndividuaisPorTurmaAnoAsync(request.TurmaCodigo, request.AnoLetivo);
    }
}
