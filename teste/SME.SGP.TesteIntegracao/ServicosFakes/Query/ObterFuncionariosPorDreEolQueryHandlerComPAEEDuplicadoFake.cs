using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterFuncionariosPorDreEolQueryHandlerComPAEEDuplicadoFake : IRequestHandler<ObterFuncionariosPorDreEolQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionariosPorDreEolQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<UsuarioEolRetornoDto>
            {
                new UsuarioEolRetornoDto
                {
                    UsuarioId = 2,
                    CodigoFuncaoAtividade = 6,
                    CodigoRf = "PAEE_1",
                    NomeServidor = "Usuario PAEE"
                },
                new UsuarioEolRetornoDto
                {
                    UsuarioId = 2,
                    CodigoFuncaoAtividade = 6,
                    CodigoRf = "PAEE_1",
                    NomeServidor = "Usuario PAEE"
                }
            });
        }
    }
}
