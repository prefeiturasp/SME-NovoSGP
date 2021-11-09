using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaUsuarioCommandHandler : IRequestHandler<SalvarPendenciaUsuarioCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaUsuario repositorioPendenciaUsuario;
        private readonly IRepositorioPendenciaPerfil repositorioPendenciaPerfil; 
        public SalvarPendenciaUsuarioCommandHandler(IMediator mediator, IRepositorioPendenciaUsuario repositorioPendenciaUsuario, IRepositorioPendenciaPerfil repositorioPendenciaPerfil)
        {
            this.mediator = mediator;
            this.repositorioPendenciaUsuario = repositorioPendenciaUsuario;
            this.repositorioPendenciaPerfil = repositorioPendenciaPerfil;
        }

        public async Task<bool> Handle(SalvarPendenciaUsuarioCommand request, CancellationToken cancellationToken)
        {
            if (request.UsuarioId == null && request.PerfilCodigo == 0)
                return false;
            
            if(request.UsuarioId != null)
            {
                await repositorioPendenciaUsuario.SalvarAsync(new PendenciaUsuario
                {
                    UsuarioId = request.UsuarioId.Value,
                    PendenciaId = request.PendenciaId
                });
            }
            
            if(request.PerfilCodigo > 0)
            {
                await repositorioPendenciaPerfil.SalvarAsync(new PendenciaPerfil
                {
                    PendenciaId = request.PendenciaId,
                    PerfilCodigo = request.PerfilCodigo,
                });

                //cria command de vinculo da pendência com usuário
            }

            return true;
        }
    }
}
