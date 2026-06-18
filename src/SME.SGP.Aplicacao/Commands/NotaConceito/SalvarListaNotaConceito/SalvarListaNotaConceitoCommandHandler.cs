using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarListaNotaConceitoCommandHandler : IRequestHandler<SalvarListaNotaConceitoCommand, bool>
    {
        private readonly IRepositorioNotasConceitos repositorioNotasConceitos;

        public SalvarListaNotaConceitoCommandHandler(IRepositorioNotasConceitos notasConceitos)
        {
            repositorioNotasConceitos = notasConceitos ?? throw new ArgumentNullException(nameof(notasConceitos));
        }

        public async Task<bool> Handle(SalvarListaNotaConceitoCommand request, CancellationToken cancellationToken)
        {
            foreach (var notaConceito in request.ListaNotasConceitos)
            {
                await repositorioNotasConceitos.SalvarNotaConceito(notaConceito);
            }
            return true;
        }
    }
}