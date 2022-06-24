using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterFuncionariosPorDreEolQueryHandlerComUmPAEEFake : IRequestHandler<ObterFuncionariosPorDreEolQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionariosPorDreEolQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<UsuarioEolRetornoDto>
                {
                    new UsuarioEolRetornoDto()
                    {
                        UsuarioId = 1,
                        CodigoFuncaoAtividade = 0,
                        CodigoRf = "1",
                        NomeServidor = "Maria da Silva"
                    },
                    new UsuarioEolRetornoDto()
                    {
                        UsuarioId = 2,
                        CodigoFuncaoAtividade = 6,
                        CodigoRf = "2",
                        NomeServidor = "Usuario PAEE"
                    }
                });
        }
    }
}
