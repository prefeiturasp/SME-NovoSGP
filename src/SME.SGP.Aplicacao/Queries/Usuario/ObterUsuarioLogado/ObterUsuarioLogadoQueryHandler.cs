using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioLogadoQueryHandler : IRequestHandler<ObterUsuarioLogadoQuery, Usuario>
    {
        private readonly IServicoUsuario servicoUsuario;

        public ObterUsuarioLogadoQueryHandler(IServicoUsuario servicoUsuario)
        {
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public Task<Usuario> Handle(ObterUsuarioLogadoQuery request, CancellationToken cancellationToken)
            => servicoUsuario.ObterUsuarioLogado();
    }
}
