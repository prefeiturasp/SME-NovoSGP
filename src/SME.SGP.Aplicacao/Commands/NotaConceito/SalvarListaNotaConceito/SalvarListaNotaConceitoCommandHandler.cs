using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class SalvarListaNotaConceitoCommandHandler : IRequestHandler<SalvarListaNotaConceitoCommand,bool>
    {
        private readonly IRepositorioNotasConceitos repositorioNotasConceitos;

        public SalvarListaNotaConceitoCommandHandler(IRepositorioNotasConceitos notasConceitos)
        {
            repositorioNotasConceitos = notasConceitos ?? throw new ArgumentNullException(nameof(notasConceitos));
        }

        public async Task<bool> Handle(SalvarListaNotaConceitoCommand request, CancellationToken cancellationToken)
        {
            return await repositorioNotasConceitos.SalvarListaNotaConceito(request.ListaNotasConceitos,request.CriadoPor);
        }
    }
}