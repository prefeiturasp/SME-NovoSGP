using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    class ObterUsuarioPorCodigoRfLoginQueryHandler : IRequestHandler<ObterUsuarioPorCodigoRfLoginQuery, Usuario>
    {
        private readonly IRepositorioUsuarioConsulta repositorioUsuarioConsulta;

        public ObterUsuarioPorCodigoRfLoginQueryHandler(IRepositorioUsuarioConsulta repositorio)
        {
            this.repositorioUsuarioConsulta = repositorio;
        }

        public async Task<Usuario> Handle(ObterUsuarioPorCodigoRfLoginQuery request, CancellationToken cancellationToken)
            => await repositorioUsuarioConsulta.ObterPorCodigoRfLogin(request.CodigoRf, request.Login);
    }
}