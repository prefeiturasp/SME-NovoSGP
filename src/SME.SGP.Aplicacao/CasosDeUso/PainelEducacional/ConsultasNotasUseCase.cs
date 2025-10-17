using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoSmeDre;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasNotasUseCase : IConsultasNotasUseCase
    {
        private readonly IMediator mediator;

        public ConsultasNotasUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<PainelEducacionalNotasVisaoSmeDreDto>> ObterNotasVisaoSmeDre(string codigoDre, int anoLetivo, int bimestre, int anoTurma)
        {
            return await mediator.Send(new ObterNotaVisaoSmeDreQuery(codigoDre, anoLetivo, bimestre, anoTurma));
        }
    }
}
