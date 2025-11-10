using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdepParaConsolidacao
{
    public class ObterProficienciaIdepParaConsolidacaoQueryHandler : IRequestHandler<ObterProficienciaIdepParaConsolidacaoQuery, IEnumerable<PainelEducacionalConsolidacaoProficienciaIdepUe>>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoProficienciaIdepConsultas _repositorioConsultas;
        public ObterProficienciaIdepParaConsolidacaoQueryHandler(IRepositorioPainelEducacionalConsolidacaoProficienciaIdepConsultas repositorioConsultas)
        {
            _repositorioConsultas = repositorioConsultas;
        }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoProficienciaIdepUe>> Handle(ObterProficienciaIdepParaConsolidacaoQuery request, CancellationToken cancellationToken)
        {
            return await _repositorioConsultas.ObterDadosParaConsolidarPorAnoAsync(request.AnoLetivo);
        }
    }
}