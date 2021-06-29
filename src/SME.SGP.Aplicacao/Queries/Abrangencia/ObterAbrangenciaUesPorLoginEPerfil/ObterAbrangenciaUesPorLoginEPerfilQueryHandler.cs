using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaUesPorLoginEPerfilQueryHandler : IRequestHandler<ObterAbrangenciaUesPorLoginEPerfilQuery, IEnumerable<AbrangenciaUeRetorno>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterAbrangenciaUesPorLoginEPerfilQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<IEnumerable<AbrangenciaUeRetorno>> Handle(ObterAbrangenciaUesPorLoginEPerfilQuery request, CancellationToken cancellationToken)
        {
            var abrangenciasUeRetorno = await repositorioAbrangencia.ObterUes(request.DreCodigo, request.Login, request.Perfil);

            return abrangenciasUeRetorno;
        }
    }
}
