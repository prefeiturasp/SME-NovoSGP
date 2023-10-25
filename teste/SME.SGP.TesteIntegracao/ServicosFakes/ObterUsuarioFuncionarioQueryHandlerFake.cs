using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterUsuarioFuncionarioQueryHandlerFake : IRequestHandler<ObterUsuarioFuncionarioQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterUsuarioFuncionarioQuery request, CancellationToken cancellationToken)
        {
            return new List<UsuarioEolRetornoDto>()
            {
                new UsuarioEolRetornoDto()
                {
                    CodigoRf = "2222222"
                }
            };
        }
    }
}
