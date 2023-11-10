using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Autenticar
{
    internal class ObterPerfisPorLoginAutenticacaoQueryHandlerFake : IRequestHandler<ObterPerfisPorLoginQuery, PerfisApiEolDto>
    {
        public async Task<PerfisApiEolDto> Handle(ObterPerfisPorLoginQuery request, CancellationToken cancellationToken)
        {
            return new PerfisApiEolDto()
            {
                CodigoRf = "PROFINF1",
                Perfis = new List<Guid>
                {
                    Perfis.PERFIL_PROFESSOR_INFANTIL,
                    Perfis.PERFIL_PROFESSOR,
                    Perfis.PERFIL_PAEE
                }
            };
        }
    }
}
