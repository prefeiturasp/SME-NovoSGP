using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesItineranciaAlunoPorIdQueryHandler : IRequestHandler<ObterQuestoesItineranciaAlunoPorIdQuery, IEnumerable<ItineranciaAlunoQuestaoDto>>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public ObterQuestoesItineranciaAlunoPorIdQueryHandler(IRepositorioItinerancia repositorioItinerancia)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
        }

        public Task<IEnumerable<ItineranciaAlunoQuestaoDto>> Handle(ObterQuestoesItineranciaAlunoPorIdQuery request, CancellationToken cancellationToken)
                => repositorioItinerancia.ObterQuestoesItineranciaAluno(request.Id);
    }
}
