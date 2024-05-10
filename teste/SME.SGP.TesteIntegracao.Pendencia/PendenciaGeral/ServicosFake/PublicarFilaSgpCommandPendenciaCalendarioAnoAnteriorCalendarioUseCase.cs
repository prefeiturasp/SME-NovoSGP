using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.PendenciaGeral.ServicosFake
{
    public class PublicarFilaSgpCommandPendenciaCalendarioAnoAnteriorCalendarioUseCase: IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IRemoverPendenciasCalendarioNoFinalDoAnoLetivoUseCase anoLetivoUseCase;

        public PublicarFilaSgpCommandPendenciaCalendarioAnoAnteriorCalendarioUseCase(IRemoverPendenciasCalendarioNoFinalDoAnoLetivoUseCase anoLetivoUseCase)
        {
            this.anoLetivoUseCase = anoLetivoUseCase ?? throw new ArgumentNullException(nameof(anoLetivoUseCase));
        }

        public async Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            return await anoLetivoUseCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(request.Filtros)));
        }
    }
}
