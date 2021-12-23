using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListagemDiariosDeBordoPorPeriodoQueryHandler : ConsultasBase, IRequestHandler<ObterListagemDiariosDeBordoPorPeriodoQuery, PaginacaoResultadoDto<DiarioBordoTituloDto>>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public ObterListagemDiariosDeBordoPorPeriodoQueryHandler(IContextoAplicacao contextoAplicacao,
                                                                 IRepositorioDiarioBordo repositorioDiarioBordo) : base(contextoAplicacao)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<PaginacaoResultadoDto<DiarioBordoTituloDto>> Handle(ObterListagemDiariosDeBordoPorPeriodoQuery request, CancellationToken cancellationToken)
        {
            var periodo = await repositorioDiarioBordo.ObterListagemDiarioBordoPorPeriodoPaginado(request.TurmaId, request.ComponenteCurricularPaiId, request.ComponenteCurricularFilhoId, request.DataInicio, request.DataFim, Paginacao);

            if (periodo == null && !periodo.Items.Any())
                return null;
            else
                return MapearParaDto(periodo);
        }

        private PaginacaoResultadoDto<DiarioBordoTituloDto> MapearParaDto(PaginacaoResultadoDto<DiarioBordoResumoDto> dto)
        {
            return new PaginacaoResultadoDto<DiarioBordoTituloDto>()
            {
                TotalPaginas = dto.TotalPaginas,
                TotalRegistros = dto.TotalRegistros,
                Items = dto.Items.Select(item => new DiarioBordoTituloDto(item.Id, item.Descricao, item.Pendente))
            };
        }
    }
}
