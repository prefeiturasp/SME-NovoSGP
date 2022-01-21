using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaPerfilUsuarioCommandHandler : AsyncRequestHandler<ExcluirPendenciaPerfilUsuarioCommand>
    {
        private readonly IRepositorioPendenciaPerfilUsuario repositorio;

        public ExcluirPendenciaPerfilUsuarioCommandHandler(IRepositorioPendenciaPerfilUsuario repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        protected override async Task Handle(ExcluirPendenciaPerfilUsuarioCommand request, CancellationToken cancellationToken)
        {
            await repositorio.Excluir(request.Id);
        }
    }
}
