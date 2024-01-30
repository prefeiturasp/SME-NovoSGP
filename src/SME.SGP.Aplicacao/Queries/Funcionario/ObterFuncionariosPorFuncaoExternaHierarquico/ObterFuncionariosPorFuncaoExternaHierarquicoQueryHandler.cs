using MediatR;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorFuncaoExternaHierarquicoQueryHandler : IRequestHandler<ObterFuncionariosPorFuncaoExternaHierarquicoQuery, IEnumerable<FuncionarioFuncaoExternaDTO>>
    {
        private readonly IMediator mediator;

        public ObterFuncionariosPorFuncaoExternaHierarquicoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<FuncionarioFuncaoExternaDTO>> Handle(ObterFuncionariosPorFuncaoExternaHierarquicoQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<FuncionarioDTO> funcionarios = null;

            funcionarios = await ObterFuncionariosPorFuncaoExterna(request.CodigoUe, (int)request.FuncaoExterna);
            var funcionariosDisponiveis = funcionarios?.Where(f => !f.EstaAfastado);
            return funcionarios.Select(f => new FuncionarioFuncaoExternaDTO(f.CodigoRF, request.FuncaoExterna));
        }

        private async Task<IEnumerable<FuncionarioDTO>> ObterFuncionariosPorFuncaoExterna(string codigoUe, int funcaoExterna)
            => await mediator.Send(new ObterFuncionariosPorUeEFuncaoExternaQuery(codigoUe, funcaoExterna));
    }
}
