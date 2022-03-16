using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasExcluidasComDiarioDeBordoAtivosQueryHandler : IRequestHandler<ObterAulasExcluidasComDiarioDeBordoAtivosQuery, IEnumerable<Aula>>
    {
        private readonly IRepositorioAulaConsulta repositorioAula;

        public ObterAulasExcluidasComDiarioDeBordoAtivosQueryHandler(IRepositorioAulaConsulta repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new System.ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<IEnumerable<Aula>> Handle(ObterAulasExcluidasComDiarioDeBordoAtivosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAula
                .ObterAulasExcluidasComDiarioDeBordoAtivos(request.CodigoTurma, request.TipoCalendarioId);
        }
    }
}
