using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioFuncionarioUseCase : IObterUsuarioFuncionarioUseCase
    {
        private readonly IMediator mediator;

        public ObterUsuarioFuncionarioUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Executar(FiltroFuncionarioDto filtroFuncionariosDto)
        {
            return await mediator.Send(new ObterUsuarioFuncionarioQuery(filtroFuncionariosDto));
        }
    }
}
