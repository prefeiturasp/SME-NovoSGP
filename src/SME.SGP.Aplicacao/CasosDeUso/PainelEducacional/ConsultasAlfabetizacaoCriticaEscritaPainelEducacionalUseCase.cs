using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresAlfabetizacaoCritica;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasAlfabetizacaoCriticaEscritaPainelEducacionalUseCase : IConsultasAlfabetizacaoCriticaEscritaPainelEducacionalUseCase
    {
        private readonly IMediator mediator;
        public ConsultasAlfabetizacaoCriticaEscritaPainelEducacionalUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<IEnumerable<PainelEducacionalIndicadorAlfabetizacaoCriticaDto>> ObterNumeroEstudantes(string codigoDre = null, string codigoUe = null)
        {
            return await mediator.Send(new PainelEducacionalIndicadoresNivelAlfabetizacaoCriticaQuery(codigoDre, codigoUe));
        }
    }
}
