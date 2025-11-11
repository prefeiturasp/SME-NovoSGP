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
        public async Task<PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto>> Handle(
            PainelEducacionalAprovacaoUeQuery request,
            CancellationToken cancellationToken)
        {
            var (registros, totalRegistros) = await repositorioPainelEducacionalAprovacaoUe.ObterAprovacao(
                request.AnoLetivo,
                request.CodigoUe,
                request.ModalidadeId,
                request.NumeroPagina,
                request.NumeroRegistros);

            var registrosPorPagina = request.NumeroRegistros <= 0 ? 10 : request.NumeroRegistros;
            var totalPaginas = (int)System.Math.Ceiling(totalRegistros / (double)registrosPorPagina);

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

            return new PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto>
            {
                Items = listaDto,
                TotalRegistros = totalRegistros,
                TotalPaginas = totalPaginas
            };
        }
    }
}
