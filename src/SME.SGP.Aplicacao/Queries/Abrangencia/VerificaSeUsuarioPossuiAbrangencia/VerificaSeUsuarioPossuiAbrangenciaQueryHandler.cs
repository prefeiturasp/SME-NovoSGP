using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Abrangencia.VerificaSeUsuarioPossuiAbrangencia
{
    public class VerificaSeUsuarioPossuiAbrangenciaQueryHandler : IRequestHandler<VerificaSeUsuarioPossuiAbrangenciaQuery, bool>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        public VerificaSeUsuarioPossuiAbrangenciaQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public Task<bool> Handle(VerificaSeUsuarioPossuiAbrangenciaQuery request, CancellationToken cancellationToken)
        {
            return repositorioAbrangencia.VerificaSeUsuarioPossuiAbrangencia(request.UsuarioRf);
        }
    }
}