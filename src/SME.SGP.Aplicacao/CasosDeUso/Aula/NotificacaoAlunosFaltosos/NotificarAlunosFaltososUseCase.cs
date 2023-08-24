using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class NotificarAlunosFaltososUseCase : INotificarAlunosFaltososUseCase
    {
        private readonly IMediator mediator;

        public NotificarAlunosFaltososUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {   
            var dresIds = await mediator.Send(ObterIdsDresQuery.Instance);

            foreach (var dreId in dresIds)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaNotificacaoAlunosFaltososDre, new DreDto(dreId)));

            return true;
        }
    }
}
