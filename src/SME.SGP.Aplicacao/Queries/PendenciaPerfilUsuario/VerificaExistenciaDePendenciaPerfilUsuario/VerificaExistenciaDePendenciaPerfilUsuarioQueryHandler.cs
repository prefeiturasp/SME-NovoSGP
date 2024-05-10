using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaExistenciaDePendenciaPerfilUsuarioQueryHandler : IRequestHandler<VerificaExistenciaDePendenciaPerfilUsuarioQuery, bool>
    {
        private readonly IRepositorioPendenciaPerfilUsuario repositorio;

        public VerificaExistenciaDePendenciaPerfilUsuarioQueryHandler(IRepositorioPendenciaPerfilUsuario repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(VerificaExistenciaDePendenciaPerfilUsuarioQuery request, CancellationToken cancellationToken)
        {
            var dadosRepositorio = await repositorio.VerificaExistencia(request.PendenciaPerfilId, request.UsuarioId);
            if (dadosRepositorio.Any())
                return true;

            return false;
        }
    }
}
