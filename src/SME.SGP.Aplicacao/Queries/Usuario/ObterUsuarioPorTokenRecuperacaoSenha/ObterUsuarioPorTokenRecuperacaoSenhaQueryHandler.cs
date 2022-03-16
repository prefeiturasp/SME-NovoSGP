using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    class ObterUsuarioPorTokenRecuperacaoSenhaQueryHandler : IRequestHandler<ObterUsuarioPorTokenRecuperacaoSenhaQuery, Usuario>
    {
        private readonly IRepositorioUsuarioConsulta repositorioUsuarioConsulta;

        public ObterUsuarioPorTokenRecuperacaoSenhaQueryHandler(IRepositorioUsuarioConsulta repositorio)
        {
            this.repositorioUsuarioConsulta = repositorio;
        }

        public async Task<Usuario> Handle(ObterUsuarioPorTokenRecuperacaoSenhaQuery request, CancellationToken cancellationToken)
            => await repositorioUsuarioConsulta.ObterPorTokenRecuperacaoSenha(request.Token);
    }
}