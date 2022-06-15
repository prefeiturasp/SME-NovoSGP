using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarUltimoLoginUsuarioCommandHandler : IRequestHandler<AtualizarUltimoLoginUsuarioCommand, bool>
    {
        private readonly IRepositorioUsuario repositorio;

        public AtualizarUltimoLoginUsuarioCommandHandler(IRepositorioUsuario repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(AtualizarUltimoLoginUsuarioCommand request, CancellationToken cancellationToken)
        {
            await repositorio.AtualizarUltimoLogin(request.Usuario.Id, request.Usuario.UltimoLogin);
            return true;
        }
    }
}
