using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorIdSemPerfilQueryHandler : IRequestHandler<ObterUsuarioPorIdSemPerfilQuery, Dominio.Usuario>
    {
        private readonly IRepositorioUsuario repositorioUsuario;

        public ObterUsuarioPorIdSemPerfilQueryHandler(IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
        }

        public async Task<Dominio.Usuario> Handle(ObterUsuarioPorIdSemPerfilQuery request, CancellationToken cancellationToken)
        {
            return await repositorioUsuario.ObterPorIdAsync(request.Id);
        }
    }
}
