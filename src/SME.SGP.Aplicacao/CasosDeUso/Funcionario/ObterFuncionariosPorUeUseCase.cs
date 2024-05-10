using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorUeUseCase : IObterFuncionariosPorUeUseCase
    {
        private readonly IMediator mediator;

        public ObterFuncionariosPorUeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Executar(string codigoUe, string filtro)
        {
            return await mediator.Send(new ObterFuncionariosPorUeQuery(codigoUe, filtro));
        }
    }
}