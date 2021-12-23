using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardFrequenciaPorAnoQueryHandler : IRequestHandler<ObterDadosDashboardFrequenciaPorAnoQuery, IEnumerable<GraficoFrequenciaGlobalPorAnoDto>>
    {
        private readonly IRepositorioConsolidacaoFrequenciaTurmaConsulta repositorioConsolidacaoFrequenciaTurma;

        public ObterDadosDashboardFrequenciaPorAnoQueryHandler(IRepositorioConsolidacaoFrequenciaTurmaConsulta repositorioConsolidacaoFrequenciaTurma)
        {
            this.repositorioConsolidacaoFrequenciaTurma = repositorioConsolidacaoFrequenciaTurma ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoFrequenciaTurma));
        }

        public async Task<IEnumerable<GraficoFrequenciaGlobalPorAnoDto>> Handle(ObterDadosDashboardFrequenciaPorAnoQuery request, CancellationToken cancellationToken)
        {
            var listaFrequencia = await repositorioConsolidacaoFrequenciaTurma.ObterFrequenciaGlobalPorAnoAsync(request.AnoLetivo, request.DreId, request.UeId, request.Modalidade, request.Semestre);
            return MontarDto(listaFrequencia);
        }

        private IEnumerable<GraficoFrequenciaGlobalPorAnoDto> MontarDto(IEnumerable<FrequenciaGlobalPorAnoDto> listaFrequencia)
        {
            var dto =  new List<GraficoFrequenciaGlobalPorAnoDto>();
            foreach(var frequencia in listaFrequencia)
            {
                if (frequencia.QuantidadeAcimaMinimoFrequencia > 0)
                {
                    dto.Add(new GraficoFrequenciaGlobalPorAnoDto()
                    {
                        Descricao = DashboardConstants.QuantidadeAcimaMinimoFrequenciaDescricao,
                        Quantidade = frequencia.QuantidadeAcimaMinimoFrequencia,
                        Turma = !string.IsNullOrEmpty(frequencia.NomeTurma) ? frequencia.NomeTurma: frequencia.Modalidade.ShortName() + " - " + frequencia.Ano
                    });
                }
                if (frequencia.QuantidadeAbaixoMinimoFrequencia > 0)
                {
                    dto.Add(new GraficoFrequenciaGlobalPorAnoDto()
                    {
                        Descricao = DashboardConstants.QuantidadeAbaixoMinimoFrequenciaDescricao,
                        Quantidade = frequencia.QuantidadeAbaixoMinimoFrequencia,
                        Turma = !string.IsNullOrEmpty(frequencia.NomeTurma) ? frequencia.NomeTurma : frequencia.Modalidade.ShortName() + " - " + frequencia.Ano
                    });
                }
            }
            return dto;
        }
    }
}
