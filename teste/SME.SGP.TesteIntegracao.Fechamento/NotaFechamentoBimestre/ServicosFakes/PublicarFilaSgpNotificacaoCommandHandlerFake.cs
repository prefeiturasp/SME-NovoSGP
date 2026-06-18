using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Fechamento.NotaFechamentoBimestre.ServicosFakes
{
    public class PublicarFilaSgpNotificacaoCommandHandlerFake : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly INotificacaoAndamentoFechamentoPorUeUseCase useCase;
        public PublicarFilaSgpNotificacaoCommandHandlerFake(INotificacaoAndamentoFechamentoPorUeUseCase useCase)
        {
            this.useCase = useCase;
        }

        public async Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            if (request.Rota != RotasRabbitSgpFechamento.RotaNotificacaoAndamentoFechamentoPorUe)
                return false;

            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(request.Filtros));

            return await useCase.Executar(mensagem);
        }
    }
}
