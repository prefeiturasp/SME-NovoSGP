using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNumeroAlunos;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasNumeroEstudantesAgrupamentoNivelAlfabetizacaoUseCase : IConsultasNumeroEstudantesAgrupamentoNivelAlfabetizacaoUseCase
    {
        private readonly IMediator mediator;

        public ConsultasNumeroEstudantesAgrupamentoNivelAlfabetizacaoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<IEnumerable<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto>> ObterNumeroEstudantes(string anoLetivo, string periodo)
        {
            return await mediator.Send(new PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery(anoLetivo, periodo));
        }
    }
}
