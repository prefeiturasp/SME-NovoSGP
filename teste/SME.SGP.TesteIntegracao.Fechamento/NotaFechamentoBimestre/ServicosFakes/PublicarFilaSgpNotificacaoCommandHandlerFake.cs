using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
