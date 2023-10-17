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
    public class ObterAutenticacaoSemSenhaQueryHandlerFake : IRequestHandler<ObterAutenticacaoSemSenhaQuery, AutenticacaoApiEolDto>
    {
        public async Task<AutenticacaoApiEolDto> Handle(ObterAutenticacaoSemSenhaQuery request, CancellationToken cancellationToken)
        {
            return new AutenticacaoApiEolDto()
            {
                CodigoRf = request.Login
            };
        }
    }
}
