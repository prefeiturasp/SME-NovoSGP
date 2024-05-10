using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorIdsSemPerfilQueryHandler : IRequestHandler<ObterUsuarioPorIdsSemPerfilQuery, IEnumerable<Dominio.Usuario>>
    {
        private readonly IRepositorioUsuario repositorioUsuario;

        public ObterUsuarioPorIdsSemPerfilQueryHandler(IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
        }

        public async Task<IEnumerable<Dominio.Usuario>> Handle(ObterUsuarioPorIdsSemPerfilQuery request, CancellationToken cancellationToken)
        {
            return await this.repositorioUsuario.ObterPorIdsAsync(request.Ids);
        }
    }
}
