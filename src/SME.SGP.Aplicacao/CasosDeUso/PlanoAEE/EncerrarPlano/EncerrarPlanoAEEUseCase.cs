using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class EncerrarPlanoAEEUseCase : IEncerrarPlanoAEEUseCase
    {
        private readonly IMediator mediator;

        public EncerrarPlanoAEEUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<RetornoEncerramentoPlanoAEEDto> Executar(long planoId)
        {
            var planoAeePersistidoDto = await mediator.Send(new EncerrarPlanoAeeCommand(planoId));
            return planoAeePersistidoDto;
        }
    }
}