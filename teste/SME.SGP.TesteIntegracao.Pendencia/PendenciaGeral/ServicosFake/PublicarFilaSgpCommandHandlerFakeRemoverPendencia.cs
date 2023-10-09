using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PendenciaGeral.ServicosFake
{
    public class PublicarFilaSgpCommandHandlerFakeRemoverPendencia : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IRemoverPendenciasNoFinalDoAnoLetivoUseCase useCase;

        public PublicarFilaSgpCommandHandlerFakeRemoverPendencia(IRemoverPendenciasNoFinalDoAnoLetivoUseCase useCase)
        {
            this.useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
        }

        public async Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            return await useCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(request.Filtros)));
        }
    }
}
