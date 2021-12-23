using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNovosTiposUEPorAnoQueryHandler : IRequestHandler<ObterNovosTiposUEPorAnoQuery, string>
    {
        private readonly IRepositorioParametrosSistemaConsulta repositorioParametrosSistema;

        public ObterNovosTiposUEPorAnoQueryHandler(IRepositorioParametrosSistemaConsulta repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema;
        }

        public async Task<string> Handle(ObterNovosTiposUEPorAnoQuery request, CancellationToken cancellationToken)
            => await repositorioParametrosSistema.ObterNovosTiposUEPorAno(request.AnoLetivo);
    }
}
