using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaPossuiObjetivosAprendizagemPorComponenteCurricularIdQueryHandler : IRequestHandler<VerificaPossuiObjetivosAprendizagemPorComponenteCurricularIdQuery, bool>
    {
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;

        public VerificaPossuiObjetivosAprendizagemPorComponenteCurricularIdQueryHandler(IRepositorioComponenteCurricular repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }
        public async Task<bool> Handle(VerificaPossuiObjetivosAprendizagemPorComponenteCurricularIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioComponenteCurricular.VerificaPossuiObjetivosAprendizagemPorComponenteCurricularId(request.ComponenteCurricularId);
        }
    }
}
