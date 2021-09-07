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
        ObterNotaPorAtividadeGoogleClassIdQueryHandler : IRequestHandler<
            ObterNotaPorAtividadeGoogleClassIdQuery, NotaConceito>
    {
        private readonly IRepositorioNotasConceitos repositorioNotasConceitos;

        public ObterNotaPorAtividadeGoogleClassIdQueryHandler(RepositorioNotasConceitos notasConceitos)
        {
            repositorioNotasConceitos = notasConceitos ?? throw new ArgumentNullException(nameof(notasConceitos));
        }

        public async Task<NotaConceito> Handle(ObterNotaPorAtividadeGoogleClassIdQuery request,
            CancellationToken cancellationToken)
            => await repositorioNotasConceitos.ObterNotasPorAtividadeIdCodigoAluno(request.AtividadeId,request.CodigoAluno);
    }
}