using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ProcessarPendenciaDevolutivaQueryHandler : IRequestHandler<ProcessarPendenciaDevolutivaQuery, bool>
    {
        private readonly IReprocessarDiarioBordoPendenciaDevolutivaPorDreUseCase pendenciaDevolutivaPorDreUseCase;
        public ProcessarPendenciaDevolutivaQueryHandler(IReprocessarDiarioBordoPendenciaDevolutivaPorDreUseCase pendenciaDevolutivaPorDreUseCase)
        {
            this.pendenciaDevolutivaPorDreUseCase = pendenciaDevolutivaPorDreUseCase ?? throw new ArgumentNullException(nameof(pendenciaDevolutivaPorDreUseCase));
        }

        public async Task<bool> Handle(ProcessarPendenciaDevolutivaQuery request, CancellationToken cancellationToken)
        {
            var msg = JsonConvert.SerializeObject(new FiltroDiarioBordoPendenciaDevolutivaDto(anoLetivo: request.AnoLetivo));
            var msgRabbit = new MensagemRabbit(msg.ToString());
            await pendenciaDevolutivaPorDreUseCase.Executar(msgRabbit);
            return true;
        }
    }
}
