using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdep
{
    public class ObterIdepsQueryHandler : IRequestHandler<ObterIdepsQuery, IEnumerable<Idep>>
    {
        private readonly IRepositorioIdep repositorioIdep;

        public ObterIdepsQueryHandler(IRepositorioIdep repositorioIdep)
        {
            this.repositorioIdep = repositorioIdep;
        }

        public async Task<IEnumerable<Idep>> Handle(ObterIdepsQuery request, CancellationToken cancellationToken)
        {
            return await repositorioIdep.ObterRegistrosIdepsAsync(request.AnoLetivo, request.CodigoEOLEscola);
        }
    }
}
