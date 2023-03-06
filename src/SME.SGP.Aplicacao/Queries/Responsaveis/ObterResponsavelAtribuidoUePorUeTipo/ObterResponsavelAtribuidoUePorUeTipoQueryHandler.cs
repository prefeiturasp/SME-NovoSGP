using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsavelAtribuidoUePorUeTipoQueryHandler : IRequestHandler<ObterResponsavelAtribuidoUePorUeTipoQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;

        public ObterResponsavelAtribuidoUePorUeTipoQueryHandler(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterResponsavelAtribuidoUePorUeTipoQuery request, CancellationToken cancellationToken)
            => await repositorioSupervisorEscolaDre.ObterResponsavelAtribuidoUePorUeTipo(request.CodigoUe, request.TipoResponsavelAtribuicao);
    }
}
