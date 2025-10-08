using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandono;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasAbandonoPainelEducacionalUseCase : IConsultasAbandonoPainelEducacionalUseCase
    {
        private readonly IMediator mediator;
        public ConsultasAbandonoPainelEducacionalUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<IEnumerable<PainelEducacionalAbandono>> ObterAbandonoVisaoSmeDre(int anoLetivo, string codigoDre, string codigoUe)
        {
            return await mediator.Send(new ObterAbandonoVisaoSmeDreQuery(anoLetivo, codigoDre, codigoUe));
        }
    }
}
