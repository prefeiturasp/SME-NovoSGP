using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaPorAtividadeGoogleClassIdQueryHandler : IRequestHandler<ObterNotaPorAtividadeGoogleClassIdQuery, NotaConceito>
    {
        private readonly IRepositorioNotasConceitosConsulta repositorioNotasConceitos;

        public ObterNotaPorAtividadeGoogleClassIdQueryHandler(IRepositorioNotasConceitosConsulta repositorioNotasConceitos)
        {
            this.repositorioNotasConceitos = repositorioNotasConceitos ?? throw new ArgumentNullException(nameof(repositorioNotasConceitos));
        }

        public async Task<NotaConceito> Handle(ObterNotaPorAtividadeGoogleClassIdQuery request, CancellationToken cancellationToken)
            => await repositorioNotasConceitos.ObterNotasPorAtividadeIdCodigoAluno(request.AtividadeId,request.CodigoAluno);
    }
}