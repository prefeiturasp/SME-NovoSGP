using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoPlanoAEE;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterConsolidacaoPlanosAEE
{
    public class ObterDadosParaConsolidarPlanosAEEQueryHandler : IRequestHandler<ObterDadosParaConsolidarPlanosAEEQuery, IEnumerable<DadosParaConsolidarPlanosAEEDto>>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorio;

        public ObterDadosParaConsolidarPlanosAEEQueryHandler(IRepositorioPlanoAEEConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<DadosParaConsolidarPlanosAEEDto>> Handle(ObterDadosParaConsolidarPlanosAEEQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorio.ObterPlanosConsolidarPainelEducacional(request.AnoLetivo);
            return registros;
        }
    }
}
