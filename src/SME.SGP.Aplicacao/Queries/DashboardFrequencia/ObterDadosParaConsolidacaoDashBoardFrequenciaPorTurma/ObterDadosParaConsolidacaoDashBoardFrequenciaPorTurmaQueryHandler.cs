using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQueryHandler : IRequestHandler<ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQuery, DadosParaConsolidacaoDashBoardFrequenciaDto>
    {
        private readonly IRepositorioDashBoardFrequencia repositorioDashBoardFrequencia;

        public ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQueryHandler(IRepositorioDashBoardFrequencia repositorioDashBoardFrequencia)
        {
            this.repositorioDashBoardFrequencia = repositorioDashBoardFrequencia ?? throw new ArgumentNullException(nameof(repositorioDashBoardFrequencia));
        }

        public async Task<DadosParaConsolidacaoDashBoardFrequenciaDto> Handle(ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioDashBoardFrequencia.ObterDadosParaConsolidacao(request.AnoLetivo,
                                                                               request.TurmaId,
                                                                               (int)request.Modalidade,
                                                                               (int)request.TipoPeriodo,
                                                                               request.DataAula,
                                                                               request.DataInicio,
                                                                               request.DataFim,
                                                                               request.Mes);
    }
}
