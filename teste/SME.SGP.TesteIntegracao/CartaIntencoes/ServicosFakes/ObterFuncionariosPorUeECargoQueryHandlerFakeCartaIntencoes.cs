using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.CartaIntencoes.ServicosFakes
{
    public class ObterFuncionariosPorUeECargoQueryHandlerFakeCartaIntencoes : IRequestHandler<ObterFuncionariosPorUeECargoQuery, IEnumerable<FuncionarioDTO>>
    {
        public Task<IEnumerable<FuncionarioDTO>> Handle(ObterFuncionariosPorUeECargoQuery request, CancellationToken cancellationToken)
            => Task.FromResult(Enumerable.Empty<FuncionarioDTO>());

    }
}
