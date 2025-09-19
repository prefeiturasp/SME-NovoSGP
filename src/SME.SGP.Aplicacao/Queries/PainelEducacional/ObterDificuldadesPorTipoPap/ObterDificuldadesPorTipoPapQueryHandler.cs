using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDificuldadesPorTipoPap
{
    public class ObterDificuldadesPorTipoPapQueryHandler : IRequestHandler<ObterDificuldadesPorTipoPapQuery, ContagemDificuldadePorTipoDto>
    {
        private readonly IRepositorioPapConsulta repositorioPapConsulta;

        public ObterDificuldadesPorTipoPapQueryHandler(IRepositorioPapConsulta repositorioPapConsulta)
        {
            this.repositorioPapConsulta = repositorioPapConsulta ?? throw new ArgumentNullException(nameof(repositorioPapConsulta));
        }

        public async Task<ContagemDificuldadePorTipoDto> Handle(ObterDificuldadesPorTipoPapQuery request, CancellationToken cancellationToken)
        {
            var resultados = await repositorioPapConsulta.ObterContagemDificuldadesPorTipoDetalhado(request.TipoPap, request.CodigoDre, request.CodigoUe);
            
            if (resultados?.Any() == true)
            {
                var primeiro = resultados.First();
                return new ContagemDificuldadePorTipoDto
                {
                    TipoPap = request.TipoPap,
                    CodigoDre = primeiro.CodigoDre,
                    NomeDre = primeiro.NomeDre,
                    CodigoUe = primeiro.CodigoUe,
                    NomeUe = primeiro.NomeUe,
                    QuantidadeEstudantesDificuldadeTop1 = resultados.Sum(x => x.QuantidadeEstudantesDificuldadeTop1),
                    QuantidadeEstudantesDificuldadeTop2 = resultados.Sum(x => x.QuantidadeEstudantesDificuldadeTop2),
                    OutrasDificuldadesAprendizagem = resultados.Sum(x => x.OutrasDificuldadesAprendizagem),
                    NomeDificuldadeTop1 = primeiro.NomeDificuldadeTop1 ?? string.Empty,
                    NomeDificuldadeTop2 = primeiro.NomeDificuldadeTop2 ?? string.Empty
                };
            }

            return new ContagemDificuldadePorTipoDto
            {
                TipoPap = request.TipoPap,
                CodigoDre = request.CodigoDre ?? string.Empty,
                NomeDre = string.Empty,
                CodigoUe = request.CodigoUe ?? string.Empty,
                NomeUe = string.Empty,
                QuantidadeEstudantesDificuldadeTop1 = 0,
                QuantidadeEstudantesDificuldadeTop2 = 0,
                OutrasDificuldadesAprendizagem = 0,
                NomeDificuldadeTop1 = string.Empty,
                NomeDificuldadeTop2 = string.Empty
            };
        }
    }
}