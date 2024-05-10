using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterFuncionariosPorCargoUeQueryHandlerFake : IRequestHandler<ObterFuncionariosPorCargoUeQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        public ObterFuncionariosPorCargoUeQueryHandlerFake(){}

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionariosPorCargoUeQuery request, CancellationToken cancellationToken)
        {
            return new List<UsuarioEolRetornoDto>{
                new UsuarioEolRetornoDto
                {
                    CodigoRf="9988776",
                    NomeServidor = "UsuarioTeste1",
                    CodigoFuncaoAtividade = 0,
                    EstaAfastado = false
                },
                new UsuarioEolRetornoDto
                {
                    CodigoRf="7788990",
                    NomeServidor = "UsuarioTeste2",
                    CodigoFuncaoAtividade = 0,
                    EstaAfastado = false
                },
            };
        }
    }
}