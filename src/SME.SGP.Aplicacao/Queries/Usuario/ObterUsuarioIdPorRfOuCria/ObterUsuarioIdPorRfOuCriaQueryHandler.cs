using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioIdPorRfOuCriaQueryHandler : IRequestHandler<ObterUsuarioIdPorRfOuCriaQuery, long>
    {
        private readonly IRepositorioUsuario repositorioUsuario;

        public ObterUsuarioIdPorRfOuCriaQueryHandler(IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
        }

        public async Task<long> Handle(ObterUsuarioIdPorRfOuCriaQuery request, CancellationToken cancellationToken)
        {
            var usuarioId = await repositorioUsuario.ObterUsuarioIdPorLoginAsync(request.UsuarioRf);

            if (usuarioId == 0)
                return await repositorioUsuario.SalvarAsync(new Usuario() { CodigoRf = request.UsuarioRf, Login = request.UsuarioRf });

            return usuarioId;
        }
    }
}
