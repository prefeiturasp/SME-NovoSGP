using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExisteResponsavelAtribuidoUePorUeTipoQueryHandler : IRequestHandler<ExisteResponsavelAtribuidoUePorUeTipoQuery, bool>
    {
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;

        public ExisteResponsavelAtribuidoUePorUeTipoQueryHandler(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }

        public async Task<bool> Handle(ExisteResponsavelAtribuidoUePorUeTipoQuery request, CancellationToken cancellationToken)
            => await repositorioSupervisorEscolaDre.ExisteResponsavelAtribuidoUePorUeTipo(request.CodigoUe, request.TipoResponsavelAtribuicao);
    }
}
