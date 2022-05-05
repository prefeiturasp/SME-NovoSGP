using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQueryHandler : IRequestHandler<ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery, IEnumerable<GraficoFrequenciaTurmaEvasaoDto>>
    {
        private readonly IRepositorioFrequenciaConsulta repositorio;

        public ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQueryHandler(IRepositorioFrequenciaConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<GraficoFrequenciaTurmaEvasaoDto>> Handle(ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterDashboardFrequenciaTurmaEvasaoAbaixo50Porcento(request.AnoLetivo, request.DreCodigo, request.UeCodigo,
                request.Modalidade, request.Semestre, request.Mes);
        }
    }
}
