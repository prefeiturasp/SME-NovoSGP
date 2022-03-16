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
    public class ObterDashboardFrequenciaAusenciasPorMotivoQueryHandler : IRequestHandler<ObterDashboardFrequenciaAusenciasPorMotivoQuery, IEnumerable<GraficoBaseDto>>
    {
        private readonly IRepositorioFrequenciaConsulta repositorio;

        public ObterDashboardFrequenciaAusenciasPorMotivoQueryHandler(IRepositorioFrequenciaConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<GraficoBaseDto>> Handle(ObterDashboardFrequenciaAusenciasPorMotivoQuery request, CancellationToken cancellationToken)
        {
            var resultadosAusenciasPorMotivo =  await repositorio.ObterDashboardFrequenciaAusenciasPorMotivo(request.AnoLetivo, request.DreId, request.UeId, request.Modalidade, request.Ano, request.TurmaId, request.Semestre);
            return resultadosAusenciasPorMotivo
                .Select(ausenciasPorMotivo =>
                {
                    if (string.IsNullOrWhiteSpace(ausenciasPorMotivo.Descricao))
                        ausenciasPorMotivo.Descricao = DashboardConstants.DescricaoMotivoPadraoParaAnotacoesSemMotivoSelecionado;

                    return ausenciasPorMotivo;
                })
                .ToList();
        }
    }
}
