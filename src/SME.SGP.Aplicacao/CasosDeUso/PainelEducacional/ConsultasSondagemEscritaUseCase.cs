using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterSondagemEscrita;
using SME.SGP.Infra.Dtos.PainelEducacional.SondagemEscrita;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasSondagemEscritaUseCase : IConsultasSondagemEscritaUseCase
    {
        private readonly IMediator mediator;

        public ConsultasSondagemEscritaUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<SondagemEscritaDto>> ObterSondagemEscrita(string codigoDre, string codigoUe, int anoLetivo, int bimestre, int serieAno)
        {
            var sondagemEscrita = await mediator.Send(new ObterSondagemEscritaQuery(codigoDre, codigoUe, anoLetivo, bimestre, serieAno));

            return sondagemEscrita;
        }
    }
}
