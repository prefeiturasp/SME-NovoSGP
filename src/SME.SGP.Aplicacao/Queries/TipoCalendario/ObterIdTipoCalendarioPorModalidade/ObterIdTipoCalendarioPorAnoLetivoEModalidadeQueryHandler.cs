using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdTipoCalendarioPorAnoLetivoEModalidadeQueryHandler : IRequestHandler<ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery, long>
    {
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;

        public ObterIdTipoCalendarioPorAnoLetivoEModalidadeQueryHandler(IRepositorioTipoCalendarioConsulta repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
        }
        public async Task<long> Handle(ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTipoCalendario.ObterIdPorAnoLetivoEModalidadeAsync(request.AnoLetivo, request.Modalidade.ObterModalidadeTipoCalendario(), request.Semestre ?? 0);
        }
    }
}
