using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class BuscarTiposCalendarioPorDescricaoUseCase : IBuscarTiposCalendarioPorDescricaoUseCase
    {
        private readonly IMediator mediator;

        public BuscarTiposCalendarioPorDescricaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<TipoCalendarioBuscaDto>> Executar(string descricao)
        {
            return await mediator.Send(new ObterTipoCalendarioPorBuscaQuery(descricao));
        }
    }
}
