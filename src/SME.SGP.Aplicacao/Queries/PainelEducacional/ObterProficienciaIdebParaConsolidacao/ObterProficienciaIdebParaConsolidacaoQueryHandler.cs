using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdebParaConsolidacao
{
    public class ObterProficienciaIdebParaConsolidacaoQueryHandler : IRequestHandler<ObterProficienciaIdebParaConsolidacaoQuery, IEnumerable<PainelEducacionalConsolidacaoProficienciaIdebUe>>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoProficienciaIdebConsultas _repositorioConsultas;
        public ObterProficienciaIdebParaConsolidacaoQueryHandler(IRepositorioPainelEducacionalConsolidacaoProficienciaIdebConsultas repositorioConsultas)
        {
            _repositorioConsultas = repositorioConsultas;
        }
        public async Task<IEnumerable<PainelEducacionalConsolidacaoProficienciaIdebUe>> Handle(ObterProficienciaIdebParaConsolidacaoQuery request, CancellationToken cancellationToken)
        {
            return await _repositorioConsultas.ObterDadosParaConsolidarPorAnoAsync(request.AnoLetivo);
        }
    }
}
