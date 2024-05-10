using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdTipoCalendarioPorAnoLetivoEModalidadeQueryHandler : IRequestHandler<ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery, long>
    {
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;
        private readonly IRepositorioCache repositorioCache;

        public ObterIdTipoCalendarioPorAnoLetivoEModalidadeQueryHandler(IRepositorioTipoCalendarioConsulta repositorioTipoCalendario, IRepositorioCache repositorioCache)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
        }
        public async Task<long> Handle(ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery request, CancellationToken cancellationToken)
        {
            var chaveCache = string.Format(NomeChaveCache.TIPO_CALENDARIO_ANO_LETIVO_MODALIDADE_SEMESTRE, request.AnoLetivo, (int)request.Modalidade.ObterModalidadeTipoCalendario(), request.Semestre ?? 0);

            return await repositorioCache
                .ObterAsync(chaveCache, async () =>
                    await repositorioTipoCalendario.ObterIdPorAnoLetivoEModalidadeAsync(request.AnoLetivo, request.Modalidade.ObterModalidadeTipoCalendario(), request.Semestre ?? 0));
        }
    }
}
