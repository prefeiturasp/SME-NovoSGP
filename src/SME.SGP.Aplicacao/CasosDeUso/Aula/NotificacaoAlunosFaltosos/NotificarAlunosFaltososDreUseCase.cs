using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class NotificarAlunosFaltososDreUseCase : INotificarAlunosFaltososDreUseCase
    {
        private readonly IMediator mediator;

        public NotificarAlunosFaltososDreUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<DreDto>();

            if (filtro.EhNulo()) 
                return false;
            
            var uesDre = await mediator.Send(new ObterUesPorDreCodigoQuery(filtro.DreCodigo));

            foreach (var ue in uesDre)
            {
                await mediator.Send(new PublicarFilaSgpCommand(
                    RotasRabbitSgpAula.RotaNotificacaoAlunosFaltososDreUe,
                    new DreUeDto(ue.DreId, ue.Id, ue.CodigoUe)));
            }

            return true;

        }
    }
}