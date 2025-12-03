using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.Abrangencia;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.ObterUsuariosPerfis
{
    public class ObterUsuariosPerfisQueryHandler : IRequestHandler<ObterUsuariosPerfisQuery, IEnumerable<AbrangenciaUsuarioPerfilDto>>
    {
        private readonly IRepositorioUsuario repositorioUsuario;

        public ObterUsuariosPerfisQueryHandler(IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioUsuario = repositorioUsuario;
        }


        public async Task<IEnumerable<AbrangenciaUsuarioPerfilDto>> Handle(ObterUsuariosPerfisQuery request, CancellationToken cancellationToken)
        {
            return await repositorioUsuario.ObterUsuariosPerfis();
        }
    }
}
