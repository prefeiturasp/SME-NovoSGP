using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverRegistroColetivoAnexoCommandHandler : IRequestHandler<RemoverRegistroColetivoAnexoCommand, bool>
    {
        private readonly IRepositorioRegistroColetivoAnexo repositorio;

        public RemoverRegistroColetivoAnexoCommandHandler(IRepositorioRegistroColetivoAnexo repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<bool> Handle(RemoverRegistroColetivoAnexoCommand request, CancellationToken cancellationToken)
        {
            return repositorio.RemoverPorRegistroColetivoId(request.RegistroColetivoId);
        }
    }
}
