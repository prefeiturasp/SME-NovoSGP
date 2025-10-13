using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDificuldadesIndicadoresPap
{
    public class ObterDificuldadesIndicadoresPapQueryHandler : IRequestHandler<ObterDificuldadesIndicadoresPapQuery, IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto>>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoIndicadoresPap repositorioConsolidacaoIndicadoresPap;

        public ObterDificuldadesIndicadoresPapQueryHandler(IRepositorioPainelEducacionalConsolidacaoIndicadoresPap repositorioConsolidacaoIndicadoresPap)
        {
            this.repositorioConsolidacaoIndicadoresPap = repositorioConsolidacaoIndicadoresPap ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoIndicadoresPap));
        }

        public async Task<IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto>> Handle(ObterDificuldadesIndicadoresPapQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConsolidacaoIndicadoresPap.ObterContagemDificuldadesConsolidadaGeral(request.DadosMatriculaAluno, cancellationToken);
        }
    }
}