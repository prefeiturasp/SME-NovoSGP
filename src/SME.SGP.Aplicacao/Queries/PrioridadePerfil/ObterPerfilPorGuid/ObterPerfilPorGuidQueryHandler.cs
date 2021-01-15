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
    public class ObterPerfilPorGuidQueryHandler : IRequestHandler<ObterPerfilPorGuidQuery, PrioridadePerfil>
    {
        private readonly IRepositorioPrioridadePerfil repositorioPrioridadePerfil;

        public ObterPerfilPorGuidQueryHandler(IRepositorioPrioridadePerfil repositorioPrioridadePerfil)
        {
            this.repositorioPrioridadePerfil = repositorioPrioridadePerfil ?? throw new ArgumentNullException(nameof(repositorioPrioridadePerfil));
        }


        public async Task<PrioridadePerfil> Handle(ObterPerfilPorGuidQuery request, CancellationToken cancellationToken)
            => await repositorioPrioridadePerfil.ObterPerfilPorId(request.Perfil);
    }
}
