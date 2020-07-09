using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPossuiAbrangenciaEmTodasAsUesQueryHandler : IRequestHandler<ObterUsuarioPossuiAbrangenciaEmTodasAsUesQuery, bool>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterUsuarioPossuiAbrangenciaEmTodasAsUesQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public Task<bool> Handle(ObterUsuarioPossuiAbrangenciaEmTodasAsUesQuery request, CancellationToken cancellationToken)
        {
            return repositorioAbrangencia.UsuarioPossuiAbrangenciaDeUmDosTipos(request.Perfil, new List<TipoPerfil> { TipoPerfil.DRE, TipoPerfil.SME });
        }
    }
}
