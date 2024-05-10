using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Notificacoes.ServicosFake
{
    public class PublicarFilaSgpTratarNotificacaoCommandHandlerFake : IRequestHandler<PublicarFilaSgpCommand, bool>
    {

        private readonly ITrataNotificacoesNiveisCargosDreUseCase niveisCargoDreUseCase;
        private readonly ITrataNotificacoesNiveisCargosUeUseCase niveisCargoUeUseCase;

        public PublicarFilaSgpTratarNotificacaoCommandHandlerFake(ITrataNotificacoesNiveisCargosDreUseCase niveisCargoDreUseCase, ITrataNotificacoesNiveisCargosUeUseCase niveisCargoUeUseCase)
        {
            this.niveisCargoDreUseCase = niveisCargoDreUseCase ?? throw new ArgumentNullException(nameof(niveisCargoDreUseCase));
            this.niveisCargoUeUseCase = niveisCargoUeUseCase ?? throw new ArgumentNullException(nameof(niveisCargoUeUseCase));
        }

        public async Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            var mensagem = new MensagemRabbit(request.Filtros);

            if (request.Rota == RotasRabbitSgpNotificacoes.Criacao)
                return true;

            if (request.Rota == RotasRabbitSgp.TratarNotificacoesNiveisCargosDre)
                return await niveisCargoDreUseCase.Executar(mensagem);

            return await niveisCargoUeUseCase.Executar(mensagem);
        }
    }
}
