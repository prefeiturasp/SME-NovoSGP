using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaPossuiObjetivosAprendizagemPorComponenteCurricularIdQueryHandler : IRequestHandler<VerificaPossuiObjetivosAprendizagemPorComponenteCurricularIdQuery, bool>
    {
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;

        public VerificaPossuiObjetivosAprendizagemPorComponenteCurricularIdQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }
        public async Task<bool> Handle(VerificaPossuiObjetivosAprendizagemPorComponenteCurricularIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioComponenteCurricular.VerificaPossuiObjetivosAprendizagemPorComponenteCurricularId(request.ComponenteCurricularId);
        }
    }
}
