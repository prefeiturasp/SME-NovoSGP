using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap
{
    public class ObterIndicadoresPapSgpQueryHandler : IRequestHandler<ObterIndicadoresPapSgpQuery, IEnumerable<ContagemDificuldadePorTipoDto>>
    {
        private readonly IRepositorioPapConsulta repositorioPapConsulta;

        public ObterIndicadoresPapSgpQueryHandler(IRepositorioPapConsulta repositorioPapConsulta)
        {
            this.repositorioPapConsulta = repositorioPapConsulta ?? throw new ArgumentNullException(nameof(repositorioPapConsulta));
        }

        public async Task<IEnumerable<ContagemDificuldadePorTipoDto>> Handle(ObterIndicadoresPapSgpQuery request, CancellationToken cancellationToken)
        {
            var indicadoresPap = new List<ContagemDificuldadePorTipoDto>();

            var tiposPap = new[] { TipoPap.PapColaborativo, TipoPap.RecuperacaoAprendizagens, TipoPap.Pap2Ano };

            foreach (var tipoPap in tiposPap)
            {
                var dificuldadesDetalhadas = await repositorioPapConsulta.ObterContagemDificuldadesPorTipoDetalhado(tipoPap, request.CodigoDre, request.CodigoUe);
                
                if (dificuldadesDetalhadas?.Any() == true)
                {
                    indicadoresPap.AddRange(dificuldadesDetalhadas);
                }
            }

            return indicadoresPap.OrderBy(x => x.TipoPap)
                                .ThenBy(x => x.CodigoDre)
                                .ThenBy(x => x.CodigoUe);
        }
    }
}