using MediatR;
using SME.SGP.Aplicacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes
{
    public class ObterUsuarioLogadoIdQueryHandlerFake : IRequestHandler<ObterUsuarioLogadoIdQuery, long>
    {
        private const long USUARIO_PAAI_LOGIN_4444444 = 4444444;
        public async Task<long> Handle(ObterUsuarioLogadoIdQuery request, CancellationToken cancellationToken)
        {
            return USUARIO_PAAI_LOGIN_4444444;
        }
    }
}
