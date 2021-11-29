using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaPerfilCommandHandler : IRequestHandler<ExcluirPendenciaPerfilCommand, bool>
    {
        private readonly IRepositorioPendenciaPerfil repositorioPendenciaPerfil;

        public ExcluirPendenciaPerfilCommandHandler(IRepositorioPendenciaPerfil repositorioPendencia)
        {
            this.repositorioPendenciaPerfil = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }
        public Task<bool> Handle(ExcluirPendenciaPerfilCommand request, CancellationToken cancellationToken)
         => repositorioPendenciaPerfil.Excluir(request.PendenciaId);
    }
}
