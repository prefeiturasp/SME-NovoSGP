using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaAulaUseCase
    {
        private readonly IMediator mediator;

        public ExcluirPendenciaAulaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(ExcluirPendenciaAulaDto dto)
        {
            return await mediator.Send(new ExcluirPendenciaAulaCommand(dto.AulaId, dto.TipoPendenciaAula));
        }
    }
}
