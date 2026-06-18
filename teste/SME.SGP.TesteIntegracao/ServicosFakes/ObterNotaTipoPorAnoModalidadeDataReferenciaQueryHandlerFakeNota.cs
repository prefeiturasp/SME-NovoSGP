using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class ObterNotaTipoPorAnoModalidadeDataReferenciaQueryHandlerFakeNota : IRequestHandler<ObterNotaTipoPorAnoModalidadeDataReferenciaQuery, NotaTipoValor>
    {
        public Task<NotaTipoValor> Handle(ObterNotaTipoPorAnoModalidadeDataReferenciaQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new NotaTipoValor() { TipoNota = TipoNota.Nota });
        }
    }
}
