using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoProficienciaIdeb;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdebParaConsolidacao;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.ProficienciaIdeb;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarProficienciaIdebPainelEducacionalUseCase : AbstractUseCase, IConsolidarProficienciaIdebPainelEducacionalUseCase
    {
        public ConsolidarProficienciaIdebPainelEducacionalUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroConsolidacaoProficienciaIdebDto>();

            var dadosConsolidados = await mediator.Send(new ObterProficienciaIdebParaConsolidacaoQuery(filtro.AnoLetivo));

            await mediator.Send(new SalvarConsolidacaoProficienciaIdebCommand(dadosConsolidados.ToList()));
            return true;
        }
    }
}
