using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class RemoverNotaConceitoCommandHandler : IRequestHandler<RemoverNotaConceitoCommand,bool>
    {
        private readonly IRepositorioNotasConceitos repositorioNotasConceitos;

        public RemoverNotaConceitoCommandHandler(IRepositorioNotasConceitos notasConceitos)
        {
            repositorioNotasConceitos = notasConceitos ?? throw new ArgumentNullException(nameof(notasConceitos));
        }

        public async Task<bool> Handle(RemoverNotaConceitoCommand request, CancellationToken cancellationToken)
        {
            await repositorioNotasConceitos.RemoverAsync(request.NotaConceito);
            return true;
        }
    }
}