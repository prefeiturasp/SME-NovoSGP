using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdepPorAnoEtapa
{
    public class ObterIdepPorAnoEtapaQueryHandler : IRequestHandler<ObterIdepPorAnoEtapaQuery, IEnumerable<PainelEducacionalIdepDto>>
    {
        private readonly IRepositorioIdepPainelEducacionalConsulta repositorioIdepConsulta;
        public ObterIdepPorAnoEtapaQueryHandler(IRepositorioIdepPainelEducacionalConsulta repositorioIdepConsulta)
        {
            this.repositorioIdepConsulta = repositorioIdepConsulta ?? throw new ArgumentNullException(nameof(repositorioIdepConsulta));
        }
        public async Task<IEnumerable<PainelEducacionalIdepDto>> Handle(ObterIdepPorAnoEtapaQuery request, CancellationToken cancellationToken)
        {
            var dadosIdep = await repositorioIdepConsulta.ObterIdepPorAnoEtapa(request.AnoLetivo, request.Etapa, request.CodigoDre);
            
            var dadosAgrupados = dadosIdep
                .GroupBy(x => new { x.AnoLetivo, x.Etapa, x.Faixa })
                .Select(g => new PainelEducacionalIdepDto
                {
                    AnoLetivo = g.Key.AnoLetivo,
                    Etapa = g.Key.Etapa,
                    Faixa = g.Key.Faixa,
                    CodigoDre = g.FirstOrDefault().CodigoDre ?? null,
                    Quantidade = g.Sum(x => x.Quantidade),
                    MediaGeral = g.Average(x => x.MediaGeral),
                    UltimaAtualizacao = g.Max(x => x.UltimaAtualizacao)
                })
                .OrderBy(x => x.AnoLetivo)
                .ThenBy(x => x.Etapa)
                .ThenBy(x => x.Faixa);

            return dadosAgrupados;
        }
    }
}
