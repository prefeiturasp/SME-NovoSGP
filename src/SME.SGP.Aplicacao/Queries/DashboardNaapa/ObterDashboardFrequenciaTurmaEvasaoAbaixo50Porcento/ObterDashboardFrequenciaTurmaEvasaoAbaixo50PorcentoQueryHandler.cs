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
            var resultado = new List<GraficoFrequenciaTurmaEvasaoDto>();

            var frequenciasTurmasEvasao = await repositorio.ObterDashboardFrequenciaTurmaEvasaoAbaixo50Porcento(request.DreCodigo, request.UeCodigo,
                request.Modalidade, request.Semestre, request.Mes);

            if (!frequenciasTurmasEvasao?.Any() ?? true)
                return resultado;

            foreach (var frequenciaTurmaEvasao in frequenciasTurmasEvasao)
            {
                var graficoFrequenciaTurmaEvasao = new GraficoFrequenciaTurmaEvasaoDto()
                {
                    Grupo = frequenciaTurmaEvasao.Grupo,
                    Descricao = DashboardConstants.QuantidadeAbaixo50PorcentoDescricao,
                    Quantidade = frequenciaTurmaEvasao.Quantidade
                };

                resultado.Add(graficoFrequenciaTurmaEvasao);
            }

            return resultado;
        }
    }
}
