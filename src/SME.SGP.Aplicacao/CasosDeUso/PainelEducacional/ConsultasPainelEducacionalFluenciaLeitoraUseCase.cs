using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFluenciaLeitora;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasPainelEducacionalFluenciaLeitoraUseCase : IConsultasPainelEducacionalFluenciaLeitoraUseCase
    {
        private readonly IMediator mediator;

        public ConsultasPainelEducacionalFluenciaLeitoraUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<PainelEducacionalFluenciaLeitoraDto>> ObterFluenciaLeitora(int periodo, int anoLetivo, string codigoDre)
        {
            return await mediator.Send(new PainelEducacionalFluenciaLeitoraQuery(periodo, anoLetivo, codigoDre));
        }
    }
}
