using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPAAIPorUeTipoResponsavelAtribuicaoQueryHandler : IRequestHandler<ObterPAAIPorUeTipoResponsavelAtribuicaoQuery, IEnumerable<ServidorDto>>
    {
        private IRepositorioSupervisorEscolaDre repositorioSupervisorEscola;
        public ObterPAAIPorUeTipoResponsavelAtribuicaoQueryHandler(IRepositorioSupervisorEscolaDre repositorioSupervisorEscola)
        {
            this.repositorioSupervisorEscola = repositorioSupervisorEscola;
        }
        public async Task<IEnumerable<ServidorDto>> Handle(ObterPAAIPorUeTipoResponsavelAtribuicaoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioSupervisorEscola.ObtemSupervisoresPorUeAsync(request.CodigoUe, request.TipoResponsavel);
        }
    }
}
