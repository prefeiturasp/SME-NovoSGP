using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Notificacoes.ServicosFake
{
    public class ObterFuncionariosCargosPorUeCargosQueryHandlerFake : IRequestHandler<ObterFuncionariosCargosPorUeCargosQuery, IEnumerable<FuncionarioCargoDTO>>
    {
        public async Task<IEnumerable<FuncionarioCargoDTO>> Handle(ObterFuncionariosCargosPorUeCargosQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<FuncionarioCargoDTO>
            {
                new FuncionarioCargoDTO()
                {
                    CargoId = Dominio.Cargo.Supervisor,
                    FuncionarioRF = "1111111"
                }
            });
        }
    }
}
