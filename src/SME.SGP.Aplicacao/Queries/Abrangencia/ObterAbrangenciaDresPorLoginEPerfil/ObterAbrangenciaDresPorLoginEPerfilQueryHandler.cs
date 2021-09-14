using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaDresPorLoginEPerfilQueryHandler : IRequestHandler<ObterAbrangenciaDresPorLoginEPerfilQuery, IEnumerable<AbrangenciaDreRetornoDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterAbrangenciaDresPorLoginEPerfilQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<IEnumerable<AbrangenciaDreRetornoDto>> Handle(ObterAbrangenciaDresPorLoginEPerfilQuery request, CancellationToken cancellationToken)
            => await repositorioAbrangencia.ObterDres(request.Login, request.Perfil);        
    }
}
