using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandono
{
    public class ObterAbandonoUeQueryHandler : IRequestHandler<ObterAbandonoUeQuery, PainelEducacionalAbandonoUeDto>
    {
        private readonly IRepositorioPainelEducacionalAbandonoUe repositorio;

        public ObterAbandonoUeQueryHandler(IRepositorioPainelEducacionalAbandonoUe repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<PainelEducacionalAbandonoUeDto> Handle(ObterAbandonoUeQuery request, CancellationToken cancellationToken)
        {
            var resultado = await repositorio.ObterAbandonoUe(request.AnoLetivo, request.CodigoDre, request.CodigoUe, request.Modalidade, request.NumeroPagina, request.NumeroRegistros);
            return MapearParaDto(resultado);
        }

        private static PainelEducacionalAbandonoUeDto MapearParaDto(IEnumerable<PainelEducacionalAbandonoUe> resultado)
        {
            var dto = new PainelEducacionalAbandonoUeDto
            {
                Modalidades = resultado.Select(r => new PainelEducacionalAbandonoTurmaDto
                {
                    Turma = r.NomeTurma,
                    QuantidadeDesistentes = r.QuantidadeDesistencias
                }).ToList(),
                TotalPaginas = resultado.FirstOrDefault()?.TotalPaginas ?? 0,
                TotalRegistros = resultado.FirstOrDefault()?.TotalRegistros ?? 0
            };
            return dto;
        }
    }
}