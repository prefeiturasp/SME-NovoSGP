using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQueryHandler : IRequestHandler<ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery, FrequenciaTurmaEvasaoDto>
    {
        private readonly IRepositorioFrequenciaConsulta repositorio;

        public ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQueryHandler(IRepositorioFrequenciaConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<FrequenciaTurmaEvasaoDto> Handle(ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterDashboardFrequenciaTurmaEvasaoAbaixo50Porcento(request.AnoLetivo, request.DreCodigo, request.UeCodigo,
                request.Modalidade, request.Semestre, request.Mes);
        }
    }
}
