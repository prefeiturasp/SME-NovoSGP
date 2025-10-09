using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandono;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional;
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
        public async Task<IEnumerable<PainelEducacionalAbandonoSmeDreDto>> ObterAbandonoVisaoSmeDre(int anoLetivo, string codigoDre, string codigoUe)
        {
            if (anoLetivo <= 0)
                throw new NegocioException("Informe o ano letivo");

            return await mediator.Send(new ObterAbandonoVisaoSmeDreQuery(anoLetivo, codigoDre, codigoUe));
        }
    }
}
