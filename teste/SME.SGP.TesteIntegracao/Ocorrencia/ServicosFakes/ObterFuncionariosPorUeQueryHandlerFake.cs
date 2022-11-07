using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Ocorrencia.ServicosFakes
{
    public class ObterFuncionariosPorUeQueryHandlerFake : IRequestHandler<ObterFuncionariosPorUeQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        private const string RF_3333333 = "3333333";
        private const string RF_4444444 = "4444444";

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionariosPorUeQuery request, CancellationToken cancellationToken)
        {
            return new List<UsuarioEolRetornoDto>()
            {
                new UsuarioEolRetornoDto()
                {
                    CodigoRf = RF_3333333,
                    NomeServidor = "Servidor 3333333",
                    Login = RF_3333333,
                    UsuarioId = 1
                },
                new UsuarioEolRetornoDto()
                {
                    CodigoRf = RF_4444444,
                    NomeServidor = "Servidor 4444444",
                    Login = RF_4444444,
                    UsuarioId = 2
                }
            };
        }
    }
}
