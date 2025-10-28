using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdeb;
using SME.SGP.Infra.Dtos.PainelEducacional.ProficienciaIdeb;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasProficienciaIdebPainelEducacionalUseCase : IConsultasProficienciaIdebPainelEducacionalUseCase
    {
        private readonly IMediator mediator;

        public ConsultasProficienciaIdebPainelEducacionalUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<PainelEducacionalProficienciaIdebDto>> ObterProficienciaIdeb(int anoLetivo, string codigoUe)
        {
            var proficienciaIdeb = await mediator.Send(new ObterProficienciaIdebQuery(anoLetivo, codigoUe));

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
