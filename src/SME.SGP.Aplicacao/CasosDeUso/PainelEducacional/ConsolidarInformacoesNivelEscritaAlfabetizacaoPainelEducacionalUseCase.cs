using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoNivelEscritaAlfabetizacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Queries.Sondagem.ObterConsolidacaoNivelEscritaAlfabetizacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarInformacoesNivelEscritaAlfabetizacaoPainelEducacionalUseCase : AbstractUseCase, IConsolidarInformacoesNivelEscritaAlfabetizacaoPainelEducacionalUseCase
    {
        public ConsolidarInformacoesNivelEscritaAlfabetizacaoPainelEducacionalUseCase(IMediator mediator) : base(mediator)
        {
            
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var dadosConsolidadosSondagem = await mediator.Send(new ObterConsolidacaoNivelEscritaAlfabetizacaoQuery());

            if (dadosConsolidadosSondagem is null)
                return false;

            await mediator.Send(new SalvarConsolidacaoNivelEscritaAlfabetizacaoPainelEducacionalCommand(dadosConsolidadosSondagem));
            return true;
        }
    }
}
