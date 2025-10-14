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
    public class ConsultasProficienciaIdebPainelEducacionalUseCase : IConsultasProficienciaIdebPainelEducacionalUseCase
    {
        private readonly IMediator mediator;

        public ConsultasProficienciaIdebPainelEducacionalUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<PainelEducacionalProficienciaIdepDto>> ObterProficienciaIdep(int anoLetivo, string codigoUe)
        {
            if (string.IsNullOrWhiteSpace(codigoUe))
                throw new NegocioException("Informe a unidade escolar");

            var proficienciaIdeb = await mediator.Send(new ObterProficienciaIdepQuery(anoLetivo, codigoUe));

            if (anoLetivo <= 0)
            {
                proficienciaIdeb = proficienciaIdeb
                    .OrderByDescending(p => p.AnoLetivo) 
                    .Take(5)
                    .ToList();
            }

            return proficienciaIdeb;
        }
    }
}
