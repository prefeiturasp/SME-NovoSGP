using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdep;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasProficienciaIdepPainelEducacionalUseCase : IConsultasProficienciaIdepPainelEducacionalUseCase
    {
        private readonly IMediator mediator;

        public ConsultasProficienciaIdepPainelEducacionalUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<PainelEducacionalProficienciaIdepDto>> ObterProficienciaIdep(int anoLetivo, string codigoUe)
        {
            if (string.IsNullOrWhiteSpace(codigoUe))
                throw new NegocioException("Informe a unidade escolar");

            var proficienciaIdep = await mediator.Send(new ObterProficienciaIdepQuery(anoLetivo, codigoUe));

            if (anoLetivo <= 0)
            {
                proficienciaIdep = proficienciaIdep
                    .OrderByDescending(p => p.AnoLetivo) 
                    .Take(5)
                    .ToList();
            }

            return proficienciaIdep;
        }
    }
}
