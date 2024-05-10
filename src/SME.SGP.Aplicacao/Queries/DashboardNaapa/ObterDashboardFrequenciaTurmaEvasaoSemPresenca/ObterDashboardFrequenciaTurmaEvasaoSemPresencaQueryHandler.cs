using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardFrequenciaTurmaEvasaoSemPresencaQueryHandler : IRequestHandler<ObterDashboardFrequenciaTurmaEvasaoSemPresencaQuery, IEnumerable<GraficoFrequenciaTurmaEvasaoDto>>
    {
        private readonly IRepositorioFrequenciaConsulta repositorio;

        public ObterDashboardFrequenciaTurmaEvasaoSemPresencaQueryHandler(IRepositorioFrequenciaConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<GraficoFrequenciaTurmaEvasaoDto>> Handle(ObterDashboardFrequenciaTurmaEvasaoSemPresencaQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterDashboardFrequenciaTurmaEvasaoSemPresenca(request.AnoLetivo, request.DreCodigo, request.UeCodigo,
                request.Modalidade, request.Semestre, request.Mes);
        }
    }
}
