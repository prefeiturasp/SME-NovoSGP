using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioRfPorIdQueryHandler : IRequestHandler<ObterUsuarioRfPorIdQuery, string>
    {
        private readonly IRepositorioUsuarioConsulta repositorioUsuario;

        public ObterUsuarioRfPorIdQueryHandler(IRepositorioUsuarioConsulta repositorioUsuario)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
        }

        public Task<string> Handle(ObterUsuarioRfPorIdQuery request, CancellationToken cancellationToken)
            => repositorioUsuario.ObterRfPorId(request.Id);
    }
}
