using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesItineranciaAlunoQueryHandler : IRequestHandler<ObterQuestoesItineranciaAlunoQuery, IEnumerable<ItineranciaAlunoQuestaoDto>>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public ObterQuestoesItineranciaAlunoQueryHandler(IRepositorioItinerancia repositorioItinerancia)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
        }

        public Task<IEnumerable<ItineranciaAlunoQuestaoDto>> Handle(ObterQuestoesItineranciaAlunoQuery request, CancellationToken cancellationToken)
                => repositorioItinerancia.ObterQuestoesItineranciaAluno(request.Id);
    }
}
