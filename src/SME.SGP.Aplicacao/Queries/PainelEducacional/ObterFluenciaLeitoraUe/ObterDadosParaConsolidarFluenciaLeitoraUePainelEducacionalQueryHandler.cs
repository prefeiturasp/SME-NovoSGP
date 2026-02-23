using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoFluenciaLeitoraUe;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFluenciaLeitoraUe
{
    public class ObterDadosParaConsolidarFluenciaLeitoraUePainelEducacionalQueryHandler : IRequestHandler<ObterDadosParaConsolidarFluenciaLeitoraUePainelEducacionalQuery, IEnumerable<ConsolidacaoFluenciaLeitoraUeDto>>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoFluenciaLeitoraUe repositorio;

        public ObterDadosParaConsolidarFluenciaLeitoraUePainelEducacionalQueryHandler(
            IRepositorioPainelEducacionalConsolidacaoFluenciaLeitoraUe repositorio)

        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<ConsolidacaoFluenciaLeitoraUeDto>> Handle(ObterDadosParaConsolidarFluenciaLeitoraUePainelEducacionalQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterDadosParaConsolidarFluenciaLeitoraUe(request.AnoLetivo);
        }
    }
}
