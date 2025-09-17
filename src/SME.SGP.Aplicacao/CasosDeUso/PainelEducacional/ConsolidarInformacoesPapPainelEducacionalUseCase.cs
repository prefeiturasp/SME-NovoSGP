using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPap;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarInformacoesPapPainelEducacionalUseCase : AbstractUseCase, IConsolidarInformacoesPapPainelEducacionalUseCase
    {
        public ConsolidarInformacoesPapPainelEducacionalUseCase(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var indicadoresPap = await mediator.Send(new ObterIndicadoresPapQuery());

            if (indicadoresPap?.Any() != true)
                return false;

            return await mediator.Send(new SalvarConsolidacaoPapCommand(indicadoresPap));
        }
    }
}
