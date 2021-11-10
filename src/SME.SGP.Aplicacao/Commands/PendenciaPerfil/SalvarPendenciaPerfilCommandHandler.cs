using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaPerfilCommandHandler : AsyncRequestHandler<SalvarPendenciaPerfilCommand>
    {
        private readonly IRepositorioPendenciaPerfil repositorioPendenciaPerfil;

        public SalvarPendenciaPerfilCommandHandler(IRepositorioPendenciaPerfil repositorioPendencia)
        {
            this.repositorioPendenciaPerfil = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }

        protected override async Task Handle(SalvarPendenciaPerfilCommand request, CancellationToken cancellationToken)
        {
            foreach(var perfil in request.PerfisCodigo)
            {
                var pendencia = new PendenciaPerfil();
                pendencia.Id = request.PendenciaId;
                pendencia.PerfilCodigo = perfil;

                await repositorioPendenciaPerfil.SalvarAsync(pendencia);
            }
        }
    }
}
