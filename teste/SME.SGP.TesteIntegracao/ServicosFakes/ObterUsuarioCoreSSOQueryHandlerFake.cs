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
    public class ObterUsuarioCoreSSOQueryHandlerFake : IRequestHandler<ObterUsuarioCoreSSOQuery, MeusDadosDto>
    {
        public async Task<MeusDadosDto> Handle(ObterUsuarioCoreSSOQuery request, CancellationToken cancellationToken)
        {
            return new MeusDadosDto()
            {
                Nome = "João Usuário",
                Email = string.Empty
            };
        }
    }
}
