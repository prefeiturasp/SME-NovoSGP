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
            if (request.UeCodigo.Equals("PAEE"))
                return new List<UsuarioEolRetornoDto>
                {
                    new UsuarioEolRetornoDto()
                    {
                        UsuarioId = 99,
                        CodigoFuncaoAtividade = 6,
                        CodigoRf = "PAEE_1",
                        EstaAfastado = false,
                        NomeServidor = "USUARIO PAEE 1"
                    }
                };

            return new List<UsuarioEolRetornoDto>
            { 
                new UsuarioEolRetornoDto()
                {   
                    UsuarioId = 1,
                    CodigoFuncaoAtividade = 0,
                    CodigoRf = "1",
                    NomeServidor = "Maria da Silva" 
                },
                
            };
        }
    }
}
