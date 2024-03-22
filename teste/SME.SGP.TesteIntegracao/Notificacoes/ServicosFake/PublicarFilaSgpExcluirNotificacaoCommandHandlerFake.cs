using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Notificacoes.ServicosFake
{
    public class PublicarFilaSgpExcluirNotificacaoCommandHandlerFake : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IExecutarExclusaoNotificacaoUseCase useCase;

        public PublicarFilaSgpExcluirNotificacaoCommandHandlerFake(IExecutarExclusaoNotificacaoUseCase useCase)
        {
            this.useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
        }

        public async Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            var mensagem = new MensagemRabbit(request.Filtros);

            return await useCase.Executar(mensagem);
        }
    }
}
