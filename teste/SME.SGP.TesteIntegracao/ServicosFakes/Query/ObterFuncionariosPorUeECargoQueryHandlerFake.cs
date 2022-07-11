using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterFuncionariosPorUeECargoQueryHandlerFake : IRequestHandler<ObterFuncionariosPorUeECargoQuery, IEnumerable<FuncionarioDTO>>
    {
        public async Task<IEnumerable<FuncionarioDTO>> Handle(ObterFuncionariosPorUeECargoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<FuncionarioDTO>
                {
                    new FuncionarioDTO()
                    {
                        CodigoRF = "1",
                        Nome = "Nome Funcionário",
                        EstaAfastado = false
                    }
                });
        }
    }
}
