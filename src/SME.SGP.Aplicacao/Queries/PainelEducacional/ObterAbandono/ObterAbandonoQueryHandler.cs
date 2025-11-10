using MediatR;
using Prometheus;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandono
{
    public class ObterAbandonoVisaoSmeDreQueryHandler : IRequestHandler<ObterAbandonoVisaoSmeDreQuery, IEnumerable<PainelEducacionalAbandonoSmeDreDto>>
    {
        private readonly IRepositorioPainelEducacionalAbandono repositorio;
        public ObterAbandonoVisaoSmeDreQueryHandler(IRepositorioPainelEducacionalAbandono repositorio)
        {
            this.repositorio = repositorio;
        }
        public async Task<IEnumerable<PainelEducacionalAbandonoSmeDreDto>> Handle(ObterAbandonoVisaoSmeDreQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorio.ObterAbandonoVisaoSmeDre(request.AnoLetivo, request.CodigoDre);
            return MapearParaDto(registros);
        }

        private IEnumerable<PainelEducacionalAbandonoSmeDreDto> MapearParaDto(IEnumerable<PainelEducacionalAbandono> registros)
        {
            var dto = new PainelEducacionalAbandonoSmeDreDto
            {
                Modalidades = registros
                               .GroupBy(r => new { r.Ano, r.Modalidade })
                               .Select(r => new PainelEducacionalAbandonoModalidadeDto
                               {
                                   Modalidade = r.Key.Modalidade,
                                   Ano = r.Key.Ano,
                                   QuantidadeDesistentes = r.Sum(x => x.QuantidadeDesistencias)
                               })
                               .OrderBy(r => r.Ano)
                               .ToList()
            };

            return new List<PainelEducacionalAbandonoSmeDreDto> { dto };
        }
    }
}
