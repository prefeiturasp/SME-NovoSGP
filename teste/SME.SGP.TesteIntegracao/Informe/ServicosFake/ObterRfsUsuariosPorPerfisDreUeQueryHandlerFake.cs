using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Informe.ServicosFake
{
    public class ObterRfsUsuariosPorPerfisDreUeQueryHandlerFake : IRequestHandler<ObterRfsUsuariosPorPerfisDreUeQuery, string[]>
    {
        public async Task<string[]> Handle(ObterRfsUsuariosPorPerfisDreUeQuery request, CancellationToken cancellationToken)
        {
            return new string[] { "1111111", "2222222" };
        }
    }
}
