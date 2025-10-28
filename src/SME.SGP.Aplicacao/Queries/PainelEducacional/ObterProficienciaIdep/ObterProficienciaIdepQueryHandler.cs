using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdep
{
    public class ObterProficienciaIdepQueryHandler : IRequestHandler<ObterProficienciaIdepQuery, IEnumerable<PainelEducacionalProficienciaIdepDto>>
    {
        private readonly IRepositorioPainelEducacionalProficienciaIdep _repositorioProficienciaIdep;
        public ObterProficienciaIdepQueryHandler(IRepositorioPainelEducacionalProficienciaIdep repositorioIdepConsulta)
        {
            _repositorioProficienciaIdep = repositorioIdepConsulta;
        }
        public async Task<IEnumerable<PainelEducacionalProficienciaIdepDto>> Handle(ObterProficienciaIdepQuery request, CancellationToken cancellationToken)
        {
            var proficiencia = await _repositorioProficienciaIdep.ObterConsolidacaoPorAnoVisaoUeAsync(PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE, request.AnoLetivo, request.CodigoUe);

           throw new NotImplementedException();
        }
    }
}
