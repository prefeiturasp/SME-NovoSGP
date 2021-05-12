using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
  public  class PublicarFilaConsolidacaoTurmasCommandHandler : IRequestHandler<PublicarFilaConsolidacaoTurmasCommand, bool>
    {
        private readonly IMediator mediator;

        public PublicarFilaConsolidacaoTurmasCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(PublicarFilaConsolidacaoTurmasCommand request, CancellationToken cancellationToken)
        {
            var mensagem = JsonConvert.SerializeObject(new ConsolidacaoTurmaDto()
            {
                Bimestre = request.Bimestre,
                TurmaId = request.TurmaId,

            });

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.ConsolidaTurmaSync,
                mensagem, 
                Guid.NewGuid(),
                null, 
                fila: RotasRabbit.ConsolidaTurmaSync));
          
            return true;
        }
    }
}
