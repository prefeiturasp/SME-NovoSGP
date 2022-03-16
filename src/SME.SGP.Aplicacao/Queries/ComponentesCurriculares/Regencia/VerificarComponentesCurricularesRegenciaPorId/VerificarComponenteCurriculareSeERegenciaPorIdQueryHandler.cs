using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
