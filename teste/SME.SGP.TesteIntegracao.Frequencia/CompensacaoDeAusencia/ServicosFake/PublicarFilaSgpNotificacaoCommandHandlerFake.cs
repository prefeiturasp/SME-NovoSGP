using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Frequencia.CompensacaoDeAusencia.ServicosFake
{
    public class PublicarFilaSgpNotificacaoCommandHandlerFake : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly INotificarCompensacaoAusenciaUseCase useCase;
        public PublicarFilaSgpNotificacaoCommandHandlerFake(INotificarCompensacaoAusenciaUseCase useCase)
        {
            this.useCase = useCase;
        }

        public async Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            if (request.Rota != RotasRabbitSgp.NotificarCompensacaoAusencia)
                return false;

            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(request.Filtros));

            return await useCase.Executar(mensagem);
        }
    }
}
