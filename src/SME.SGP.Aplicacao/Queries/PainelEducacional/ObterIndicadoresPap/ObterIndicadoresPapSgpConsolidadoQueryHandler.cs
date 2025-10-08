using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap
{
    public class ObterIndicadoresPapSgpConsolidadoQueryHandler : IRequestHandler<ObterIndicadoresPapSgpConsolidadoQuery, IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto>>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoIndicadoresPap repositorioConsolidacaoIndicadoresPap;

        public ObterIndicadoresPapSgpConsolidadoQueryHandler(IRepositorioPainelEducacionalConsolidacaoIndicadoresPap repositorioConsolidacaoIndicadoresPap)
        {
            this.repositorioConsolidacaoIndicadoresPap = repositorioConsolidacaoIndicadoresPap ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoIndicadoresPap));
        }

        public async Task<IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto>> Handle(ObterIndicadoresPapSgpConsolidadoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConsolidacaoIndicadoresPap.ObterContagemDificuldadesConsolidadaGeral(request.DadosMatriculaAluno, cancellationToken);
        }
    }
}
