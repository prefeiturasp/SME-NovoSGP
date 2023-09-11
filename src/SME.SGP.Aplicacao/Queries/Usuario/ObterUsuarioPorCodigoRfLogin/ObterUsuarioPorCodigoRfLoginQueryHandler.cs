using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    class ObterUsuarioPorCodigoRfLoginQueryHandler : IRequestHandler<ObterUsuarioPorCodigoRfLoginQuery, Usuario>
    {
        private readonly IRepositorioUsuarioConsulta repositorioUsuarioConsulta;
        private readonly IRepositorioCache repositorioCache;

        public ObterUsuarioPorCodigoRfLoginQueryHandler(IRepositorioUsuarioConsulta repositorio, IRepositorioCache repositorioCache)
        {
            this.repositorioUsuarioConsulta = repositorio;
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<Usuario> Handle(ObterUsuarioPorCodigoRfLoginQuery request, CancellationToken cancellationToken)
        {
            var valorChave = !string.IsNullOrEmpty(request.CodigoRf) ? 
                                request.CodigoRf : 
                                request.Login;
            var chaveCache = string.Format(NomeChaveCache.USUARIO, valorChave);

            return await repositorioCache.ObterAsync<Usuario>(chaveCache, 
                () => repositorioUsuarioConsulta.ObterPorCodigoRfLogin(request.CodigoRf, request.Login));
        }
    }
}