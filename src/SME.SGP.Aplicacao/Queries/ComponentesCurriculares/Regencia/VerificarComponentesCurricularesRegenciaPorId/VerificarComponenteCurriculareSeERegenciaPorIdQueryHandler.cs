using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificarComponenteCurriculareSeERegenciaPorIdQueryHandler : IRequestHandler<VerificarComponenteCurriculareSeERegenciaPorIdQuery, bool>
    {
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        public VerificarComponenteCurriculareSeERegenciaPorIdQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }

        public async Task<bool> Handle(VerificarComponenteCurriculareSeERegenciaPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioComponenteCurricular.VerificarComponenteCurriculareSeERegenciaPorId(request.Id);
        }
    }
}
