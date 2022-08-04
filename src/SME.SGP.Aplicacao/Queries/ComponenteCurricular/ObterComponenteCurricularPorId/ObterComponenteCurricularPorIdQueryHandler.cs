using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponenteCurricularPorIdQueryHandler : IRequestHandler<ObterComponenteCurricularPorIdQuery, DisciplinaDto>
    {
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;

        public ObterComponenteCurricularPorIdQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }

        public async Task<DisciplinaDto> Handle(ObterComponenteCurricularPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioComponenteCurricular.ObterDisciplinaPorId(request.ComponenteCurricularId);
    }
}
