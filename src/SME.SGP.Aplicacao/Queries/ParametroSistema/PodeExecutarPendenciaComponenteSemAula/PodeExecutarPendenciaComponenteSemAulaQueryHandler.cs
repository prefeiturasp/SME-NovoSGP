using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PodeExecutarPendenciaComponenteSemAulaQueryHandler : IRequestHandler<PodeExecutarPendenciaComponenteSemAulaQuery, bool>
    {
        private readonly IRepositorioParametrosSistemaConsulta repositorioParametrosSistema;

        public PodeExecutarPendenciaComponenteSemAulaQueryHandler(IRepositorioParametrosSistemaConsulta repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema;
        }

        public async Task<bool> Handle(PodeExecutarPendenciaComponenteSemAulaQuery request, CancellationToken cancellationToken)
        {
            var dadosParametro = await repositorioParametrosSistema.ObterParametroPorTipoEAno(TipoParametroSistema.ExecutaPendenciaComponenteSemAula, DateTimeExtension.HorarioBrasilia().Year);

            return dadosParametro?.Ativo ?? false; 
        }
    }
}
