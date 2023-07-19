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

        public DiasAposInicioPeriodoLetivoComponenteSemAulaQueryHandler(IRepositorioParametrosSistemaConsulta repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema;
        }

        public async Task<int> Handle(DiasAposInicioPeriodoLetivoComponenteSemAulaQuery request, CancellationToken cancellationToken)
        {
            var dadosParametro = await repositorioParametrosSistema.ObterParametroPorTipoEAno(TipoParametroSistema.DiasAposInicioPeriodoLetivoComponenteSemAula, DateTimeExtension.HorarioBrasilia().Year);

            return dadosParametro != null && dadosParametro.Ativo ? int.Parse(dadosParametro.Valor) : 0;
        }
    }
}
