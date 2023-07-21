using MediatR;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosAnoAtualUseCase : IObterComunicadosAnoAtualUseCase
    {
        private readonly IMediator mediator;

        public ObterComunicadosAnoAtualUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public Task<IEnumerable<ComunicadoTurmaAlunoDto>> Executar()
        {
            return mediator.Send(new ObterComunicadosAnoAtualQuery());
        }
    }
}
