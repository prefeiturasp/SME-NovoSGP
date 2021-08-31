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
        ObterAtividadeNotaConseitoPorAtividadeGoogleClassIdHandler : IRequestHandler<
            ObterAtividadeNotaConseitoPorAtividadeGoogleClassIdQuery, NotaConceito>
    {
        private readonly IRepositorioNotasConceitos repositorioNotasConceitos;

        public ObterAtividadeNotaConseitoPorAtividadeGoogleClassIdHandler(RepositorioNotasConceitos notasConceitos)
        {
            repositorioNotasConceitos = notasConceitos ?? throw new ArgumentNullException(nameof(notasConceitos));
        }

        public async Task<NotaConceito> Handle(ObterAtividadeNotaConseitoPorAtividadeGoogleClassIdQuery request,
            CancellationToken cancellationToken)
            => await repositorioNotasConceitos.ObterNotasPorGoogleClassroomIdTruemaIdComponentCurricularId(request.AtividadeGoogleClassroomId,
                request.TurmaId, request.componenteCurricularId);
    }
}