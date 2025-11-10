using MediatR;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaGlobal;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaRanking
{
    public class PainelEducacionalVisaoGeralQueryHandler : IRequestHandler<PainelEducacionalVisaoGeralQuery, IEnumerable<PainelEducacionalVisaoGeralRetornoDto>>
    {
        private readonly IRepositorioPainelEducacionalVisaoGeral repositorioPainelEducacionalVisaoGeral;
        public PainelEducacionalVisaoGeralQueryHandler(IRepositorioPainelEducacionalVisaoGeral repositorioPainelEducacionalVisaoGeral)
        {
            this.repositorioPainelEducacionalVisaoGeral = repositorioPainelEducacionalVisaoGeral;
        }


        public async Task<IEnumerable<PainelEducacionalVisaoGeralRetornoDto>> Handle(PainelEducacionalVisaoGeralQuery request, CancellationToken cancellationToken)
        {
            if (request.AnoLetivo <= 0)
            {
                throw new NegocioException("Informe o ano letivo");
            }

            if (request.CodigoDre == "-99")
                request.CodigoDre = "";

            if (request.CodigoUe == "-99")
                request.CodigoUe = "";

            var registros = await repositorioPainelEducacionalVisaoGeral.ObterVisaoGeralConsolidada(request.AnoLetivo, request.CodigoDre, request.CodigoUe);

            return MapearParaDto(registros);
        }

        private IEnumerable<PainelEducacionalVisaoGeralRetornoDto> MapearParaDto(
            IEnumerable<PainelEducacionalVisaoGeralDto> registros)
        {
            return registros
                .GroupBy(r => r.Indicador)
                .Select(g => new PainelEducacionalVisaoGeralRetornoDto
                {
                    Indicador = g.Key,
                    Series = g.GroupBy(r => new { r.Serie })
                              .Select(s => new PainelEducacionalSerieDto
                              {
                                  Serie = s.Key.Serie,
                                  Valor = Math.Round(s.Average(x => x.Valor), 2, MidpointRounding.AwayFromZero)
                              })
                              .ToList()
                })
                .ToList();
        }
    }
}
