using MediatR;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarInformacoesDoAtendimentoNAAPAUseCase : AbstractUseCase, IAtualizarInformacoesDoAtendimentoNAAPAUseCase
    {
        public AtualizarInformacoesDoAtendimentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var encaminhamentos = await mediator.Send(ObterAtendimentosComSituacaoDiferenteDeEncerradoQuery.Instance);

            foreach (var encaminhamento in encaminhamentos) 
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarAtualizacaoDaTurmaDoEncaminhamentoNAAPA, encaminhamento, Guid.NewGuid(), null));
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarAtualizacaoDoEnderecoDoEncaminhamentoNAAPA, encaminhamento, Guid.NewGuid(), null));
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarAtualizacaoDasTurmasProgramaDoEncaminhamentoNAAPA, encaminhamento, Guid.NewGuid(), null));
            }

            return true;
        }
    }
}
