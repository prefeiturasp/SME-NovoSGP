using MediatR;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarInformacoesDoPlanoAEEUseCase : AbstractUseCase, IAtualizarInformacoesDoPlanoAEEUseCase
    {
        public AtualizarInformacoesDoPlanoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var planos = await mediator.Send(new ObterEncaminhamentosComSituacaoDiferenteDeEncerradoQuery());
            foreach (var plano in planos) 
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ExecutarAtualizacaoDaTurmaDoPlanoAEE, plano, Guid.NewGuid(), null));
            return true;
        }
    }
}
