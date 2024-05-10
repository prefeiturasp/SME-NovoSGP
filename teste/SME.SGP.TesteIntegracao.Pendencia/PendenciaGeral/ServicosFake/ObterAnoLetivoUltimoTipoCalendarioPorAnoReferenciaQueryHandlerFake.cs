using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;

namespace SME.SGP.TesteIntegracao.PendenciaGeral.ServicosFake
{
    public class ObterAnoLetivoUltimoTipoCalendarioPorAnoReferenciaQueryHandlerFake : IRequestHandler<ObterAnoLetivoUltimoTipoCalendarioPorAnoReferenciaQuery, int>
    {

        public async Task<int> Handle(ObterAnoLetivoUltimoTipoCalendarioPorAnoReferenciaQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year);
        }
    }
}