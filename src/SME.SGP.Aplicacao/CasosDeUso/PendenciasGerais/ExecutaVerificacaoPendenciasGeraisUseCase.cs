using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ExecutaVerificacaoPendenciasGeraisUseCase : IExecutaVerificacaoPendenciasGeraisUseCase
    {
        private readonly IMediator mediator;

        public ExecutaVerificacaoPendenciasGeraisUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public void Executar()
        {
            
        }
    }
}
