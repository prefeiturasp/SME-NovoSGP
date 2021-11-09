using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaPerfilCommandHandler : IRequestHandler<SalvarPendenciaPerfilCommand, List<long>>
    {
        private readonly IRepositorioPendenciaPerfil repositorioPendenciaPerfil;

        public SalvarPendenciaPerfilCommandHandler(IRepositorioPendenciaPerfil repositorioPendencia)
        {
            this.repositorioPendenciaPerfil = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }

        public async Task<List<long>> Handle(SalvarPendenciaPerfilCommand request, CancellationToken cancellationToken)
        {
            var listaIdsPerfilUsuario = new List<long>();
            foreach(var perfil in request.PerfisCodigo)
            {
                var pendencia = new PendenciaPerfil();
                pendencia.Id = request.PendenciaId;
                pendencia.PerfilCodigo = perfil;

                listaIdsPerfilUsuario.Add(await repositorioPendenciaPerfil.SalvarAsync(pendencia));
            }
           return listaIdsPerfilUsuario;
        }
    }
}
