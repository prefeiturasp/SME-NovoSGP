using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAlfabetizacaoCriticaEscrita;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Queries.Sondagem.ObterConsolidacaoAlfabetizacaoCriticaEscrita;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarInformacoesAlfabetizacaoCriticaEscritaPainelEducacionalUseCase: AbstractUseCase, IConsolidarInformacoesAlfabetizacaoCriticaEscritaPainelEducacionalUseCase
    {
        public ConsolidarInformacoesAlfabetizacaoCriticaEscritaPainelEducacionalUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<bool> Executar(MensagemRabbit param)
        {
            var dadosConsolidadosSondagem = await mediator.Send(new ObterConsolidacaoAlfabetizacaoCriticaEscritaQuery());
            if (dadosConsolidadosSondagem is null)
                return false;
            await mediator.Send(new SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommand(dadosConsolidadosSondagem));
            return true;
        }
    }
}
