using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoUe
{
    public class PainelEducacionalAprovacaoUeQueryHandler
        : IRequestHandler<PainelEducacionalAprovacaoUeQuery, PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto>>
    {
        private readonly IRepositorioPainelEducacionalAprovacaoUe repositorioPainelEducacionalAprovacaoUe;

        public PainelEducacionalAprovacaoUeQueryHandler(IRepositorioPainelEducacionalAprovacaoUe repositorioPainelEducacionalAprovacaoUe)
        {
            this.repositorioPainelEducacionalAprovacaoUe = repositorioPainelEducacionalAprovacaoUe;
        }

        public async Task<PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto>> Handle(PainelEducacionalAprovacaoUeQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorioPainelEducacionalAprovacaoUe.ObterAprovacao(request.AnoLetivo, request.CodigoUe, request.Modalidade, request.NumeroPagina, request.NumeroRegistros);

            if (!string.IsNullOrEmpty(request.Modalidade))
                registros = registros.Where(r => r.Modalidade?.Equals(request.Modalidade, System.StringComparison.OrdinalIgnoreCase) == true);

            var listaDto = registros.Select(item => new PainelEducacionalAprovacaoUeDto
            {
                CodigoDre = item.CodigoDre,
                CodigoUe = item.CodigoUe,
                Turma = item.Turma,
                Modalidade = item.Modalidade,
                TotalPromocoes = item.TotalPromocoes,
                TotalRetencoesAusencias = item.TotalRetencoesAusencias,
                TotalRetencoesNotas = item.TotalRetencoesNotas,
                AnoLetivo = item.AnoLetivo
            }).ToList();

            var totalRegistros = listaDto.Count;

            var pagina = request.NumeroPagina <= 0 ? 1 : request.NumeroPagina;
            var registrosPorPagina = request.NumeroRegistros <= 0 ? 10 : request.NumeroRegistros;

            var itensPaginados = listaDto
                .Skip((pagina - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
                .ToList();

            var totalPaginas = (int)System.Math.Ceiling((double)totalRegistros / registrosPorPagina);

            return new PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto>
            {
                Items = itensPaginados,
                TotalRegistros = totalRegistros,
                TotalPaginas = totalPaginas
            };
        }
    }
}
