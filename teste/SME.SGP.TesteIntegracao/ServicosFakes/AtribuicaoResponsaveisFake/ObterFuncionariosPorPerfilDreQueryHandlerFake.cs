using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class ObterFuncionariosPorPerfilDreQueryHandlerFake : IRequestHandler<ObterFuncionariosPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionariosPorPerfilDreQuery request, CancellationToken cancellationToken)
        {
                                                                    
            return new List<UsuarioEolRetornoDto>
            { new UsuarioEolRetornoDto()
            {   UsuarioId = 1,
                CodigoFuncaoAtividade = 0,
                CodigoRf = "1",
                EstaAfastado = false,
                NomeServidor = "Jose Teste" }
            };
        }
    }
}
