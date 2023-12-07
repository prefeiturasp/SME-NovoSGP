using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosComponentesCurricularesRegenciaAulasAutomaticasQueryHandler : IRequestHandler<ObterCodigosComponentesCurricularesRegenciaAulasAutomaticasQuery, string[]>
    {
        public async Task<string[]> Handle(ObterCodigosComponentesCurricularesRegenciaAulasAutomaticasQuery request, CancellationToken cancellationToken)
        {
            if (request.Modalidade == Modalidade.Fundamental)
                return await Task.FromResult(new string[] { "508", "1105", "1112", "1115", "1117", "1121", "1124", "1211", "1212", "1213", "1290", "1301" });

            if (request.Modalidade == Modalidade.EJA)
                return await Task.FromResult(new string[] { "1113", "1114", "1125" });

            return await Task.FromResult(Array.Empty<string>());
        }
    }
}
