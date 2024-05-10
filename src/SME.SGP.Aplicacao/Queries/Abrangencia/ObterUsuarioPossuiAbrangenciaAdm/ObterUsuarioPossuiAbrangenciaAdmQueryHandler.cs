using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPossuiAbrangenciaAdmQueryHandler : IRequestHandler<ObterUsuarioPossuiAbrangenciaAdmQuery, bool>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterUsuarioPossuiAbrangenciaAdmQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<bool> Handle(ObterUsuarioPossuiAbrangenciaAdmQuery request, CancellationToken cancellationToken)
            => await repositorioAbrangencia.UsuarioPossuiAbrangenciaAdm(request.UsuarioId);
    }
}
