using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class
        ObterNotasPorGoogleClassroomIdTurmaIdComponentCurricularIdHandler : IRequestHandler<
            ObterAtividadePorAtividadeGoogleClassIdQuery, NotaConceito>
    {
        private readonly IRepositorioNotasConceitos repositorioNotasConceitos;

        public ObterNotasPorGoogleClassroomIdTurmaIdComponentCurricularIdHandler(RepositorioNotasConceitos notasConceitos)
        {
            repositorioNotasConceitos = notasConceitos ?? throw new ArgumentNullException(nameof(notasConceitos));
        }

        public async Task<NotaConceito> Handle(ObterAtividadePorAtividadeGoogleClassIdQuery request,
            CancellationToken cancellationToken)
            => await repositorioNotasConceitos.ObterNotasPorGoogleClassroomIdTruemaIdComponentCurricularId(request.AtividadeGoogleClassroomId,
                request.TurmaId, request.componenteCurricularId);
    }
}