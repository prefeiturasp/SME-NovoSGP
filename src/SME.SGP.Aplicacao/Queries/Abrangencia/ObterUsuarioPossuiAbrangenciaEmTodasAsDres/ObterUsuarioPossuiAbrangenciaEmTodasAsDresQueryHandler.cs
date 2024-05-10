using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPossuiAbrangenciaEmTodasAsDresQueryHandler : IRequestHandler<ObterUsuarioPossuiAbrangenciaEmTodasAsDresQuery, bool>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterUsuarioPossuiAbrangenciaEmTodasAsDresQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public Task<bool> Handle(ObterUsuarioPossuiAbrangenciaEmTodasAsDresQuery request, CancellationToken cancellationToken)
        {
            return repositorioAbrangencia.UsuarioPossuiAbrangenciaDeUmDosTipos(request.Perfil, new List<TipoPerfil> { TipoPerfil.SME });
        }
    }
}
