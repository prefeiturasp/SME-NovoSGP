using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardAusenciasComJustificativaQueryHandler : IRequestHandler<ObterDadosDashboardAusenciasComJustificativaQuery, IEnumerable<GraficoAusenciasComJustificativaResultadoDto>>
    {
        private readonly IRepositorioConsolidacaoFrequenciaTurmaConsulta repositorioConsolidacaoFrequenciaTurma;

        public ObterDadosDashboardAusenciasComJustificativaQueryHandler(IRepositorioConsolidacaoFrequenciaTurmaConsulta repositorioConsolidacaoFrequenciaTurma)
        {
            this.repositorioConsolidacaoFrequenciaTurma = repositorioConsolidacaoFrequenciaTurma ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoFrequenciaTurma));
        }

        public async Task<IEnumerable<GraficoAusenciasComJustificativaResultadoDto>> Handle(ObterDadosDashboardAusenciasComJustificativaQuery request, CancellationToken cancellationToken)
        {
            var ausenciasComJustificativa = await repositorioConsolidacaoFrequenciaTurma.ObterAusenciasComJustificativaASync(request.AnoLetivo, request.DreId, request.UeId, request.Modalidade, request.Semstre);
            return MontarDto(ausenciasComJustificativa);
        }

        private IEnumerable<GraficoAusenciasComJustificativaResultadoDto> MontarDto(IEnumerable<GraficoAusenciasComJustificativaDto> ausenciasComJustificativa)
        {
            var dto = new List<GraficoAusenciasComJustificativaResultadoDto>();
            foreach (var ausencia in ausenciasComJustificativa)
            {
                dto.Add(new GraficoAusenciasComJustificativaResultadoDto()
                {
                    Descricao = string.IsNullOrEmpty(ausencia.NomeTurma) ? $"{ausencia.Modalidade.ShortName()}-{ausencia.Ano}" : ausencia.NomeTurma,
                    Quantidade = ausencia.Quantidade,
                    ModalidadeAno = string.IsNullOrEmpty(ausencia.NomeTurma) ? $"{ausencia.Modalidade.ShortName()}-{ausencia.Ano}" : ausencia.NomeTurma,
                });
            }
            return dto;
        }
    }
}
