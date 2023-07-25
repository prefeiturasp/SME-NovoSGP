using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQueryHandler : IRequestHandler<ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQuery, IEnumerable<DadosParaConsolidacaoDashBoardFrequenciaDto>>
    {
        private readonly IRepositorioDashBoardFrequencia repositorioDashBoardFrequencia;

        public ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQueryHandler(IRepositorioDashBoardFrequencia repositorioDashBoardFrequencia)
        {
            this.repositorioDashBoardFrequencia = repositorioDashBoardFrequencia ?? throw new ArgumentNullException(nameof(repositorioDashBoardFrequencia));
        }

        public async Task<IEnumerable<DadosParaConsolidacaoDashBoardFrequenciaDto>> Handle(ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioDashBoardFrequencia.ObterDadosParaConsolidacao(request.AnoLetivo,
                                                                               request.TurmaId,
                                                                               (int)request.Modalidade,
                                                                               request.DataInicio,
                                                                               request.DataFim,
                                                                               request.DataAula);
    }
}
