using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoProficienciaIdep;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdepParaConsolidacao;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.ProficienciaIdeb;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarProficienciaIdepPainelEducacionalUseCase : AbstractUseCase, IConsolidarProficienciaIdepPainelEducacionalUseCase
    {
        public ConsolidarProficienciaIdepPainelEducacionalUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroConsolidacaoProficienciaIdepDto>();

            var dadosConsolidados = await mediator.Send(new ObterProficienciaIdepParaConsolidacaoQuery(filtro.AnoLetivo));

            await mediator.Send(new SalvarConsolidacaoProficienciaIdepCommand(dadosConsolidados.ToList()));
            return true;
        }
    }
}
