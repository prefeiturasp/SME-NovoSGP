using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverRegistroColetivoUeCommandHandler : IRequestHandler<RemoverRegistroColetivoUeCommand, bool>
    {
        private readonly IRepositorioRegistroColetivoUe repositorio;

        public RemoverRegistroColetivoUeCommandHandler(IRepositorioRegistroColetivoUe repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<bool> Handle(RemoverRegistroColetivoUeCommand request, CancellationToken cancellationToken)
        {
            return repositorio.RemoverPorRegistroColetivoId(request.RegistroColetivoId);
        }
    }
}
