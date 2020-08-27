using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.DiarioBordo.ObterDiariosDeBordoPorPeriodo
{
    public class ObterDiariosDeBordoPorPeriodoQueryHandler : ConsultasBase, IRequestHandler<ObterDiariosDeBordoPorPeriodoQuery, PaginacaoResultadoDto<DiarioBordoDevolutivaDto>>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public ObterDiariosDeBordoPorPeriodoQueryHandler(IContextoAplicacao contextoAplicacao,
                                                         IRepositorioDiarioBordo repositorioDiarioBordo) : base(contextoAplicacao)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>> Handle(ObterDiariosDeBordoPorPeriodoQuery request, CancellationToken cancellationToken)
            => await repositorioDiarioBordo.ObterDiariosBordoPorPeriodoPaginado(request.TurmaCodigo, request.ComponenteCurricularCodigo, request.PeriodoInicio, request.PeriodoFim, Paginacao);
    }
}
