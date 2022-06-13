using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterFuncionariosPorDreEolQueryHandlerFake : IRequestHandler<ObterFuncionariosPorDreEolQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionariosPorDreEolQuery request, CancellationToken cancellationToken)
        {
            return new List<UsuarioEolRetornoDto>
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
                   UsuarioId = 1111111,
                    CodigoFuncaoAtividade = 0,
                    CodigoRf = "1111111",
                    EstaAfastado = false,
                    NomeServidor = "USUARIO LOGADO 1111111" 
               }
           };
        }
    }
}
