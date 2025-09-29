using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresAlfabetizacaoCritica;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
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
        public async Task<IEnumerable<PainelEducacionalIndicadorAlfabetizacaoCriticaDto>> ObterNumeroEstudantes(int anoLetivo, string codigoDre = null, string codigoUe = null)
        {
            if (anoLetivo == 0) anoLetivo = DateTime.Now.Year;
            return await mediator.Send(new PainelEducacionalIndicadoresNivelAlfabetizacaoCriticaQuery(anoLetivo, codigoDre, codigoUe));
        }
    }
}
