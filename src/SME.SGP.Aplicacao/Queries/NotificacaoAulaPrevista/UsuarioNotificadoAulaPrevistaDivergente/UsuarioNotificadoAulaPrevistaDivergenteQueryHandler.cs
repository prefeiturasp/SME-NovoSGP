using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class UsuarioNotificadoAulaPrevistaDivergenteQueryHandler : IRequestHandler<UsuarioNotificadoAulaPrevistaDivergenteQuery, bool>
    {
        private readonly IRepositorioNotificacaoAulaPrevista repositorioNotificacaoAulaPrevista;

        public UsuarioNotificadoAulaPrevistaDivergenteQueryHandler(IRepositorioNotificacaoAulaPrevista repositorioNotificacaoAulaPrevista)
        {
            this.repositorioNotificacaoAulaPrevista = repositorioNotificacaoAulaPrevista ?? throw new ArgumentNullException(nameof(repositorioNotificacaoAulaPrevista));
        }

        public async Task<bool> Handle(UsuarioNotificadoAulaPrevistaDivergenteQuery request, CancellationToken cancellationToken)
            => await repositorioNotificacaoAulaPrevista.UsuarioNotificado(request.UsuarioId, request.Bimestre, request.TurmaCodigo, request.ComponenteCurricularId);
    }
}
