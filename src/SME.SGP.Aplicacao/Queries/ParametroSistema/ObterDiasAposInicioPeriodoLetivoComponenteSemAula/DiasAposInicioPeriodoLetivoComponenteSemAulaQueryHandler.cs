using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class DiasAposInicioPeriodoLetivoComponenteSemAulaQueryHandler : IRequestHandler<DiasAposInicioPeriodoLetivoComponenteSemAulaQuery, int>
    {
        private readonly IRepositorioParametrosSistemaConsulta repositorioParametrosSistema;
        private readonly IRepositorioCache repositorioCache;

        public DiasAposInicioPeriodoLetivoComponenteSemAulaQueryHandler(IRepositorioParametrosSistemaConsulta repositorioParametrosSistema, IRepositorioCache repositorioCache)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new System.ArgumentNullException(nameof(repositorioParametrosSistema));
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<int> Handle(DiasAposInicioPeriodoLetivoComponenteSemAulaQuery request, CancellationToken cancellationToken)
        {
            var nomeChave = $"parametro-{TipoParametroSistema.DiasAposInicioPeriodoLetivoComponenteSemAula}-{DateTimeExtension.HorarioBrasilia().Year}";
            var dadosParametro = await repositorioCache.ObterAsync(nomeChave, () => repositorioParametrosSistema.ObterParametroPorTipoEAno(TipoParametroSistema.DiasAposInicioPeriodoLetivoComponenteSemAula, DateTimeExtension.HorarioBrasilia().Year));

            return dadosParametro != null && dadosParametro.Ativo ? int.Parse(dadosParametro.Valor) : 0;
        }
    }
}
