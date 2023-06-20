using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class ObterNotaTipoPorAnoModalidadeDataReferenciaQueryHandlerFakeNota : IRequestHandler<ObterNotaTipoPorAnoModalidadeDataReferenciaQuery, NotaTipoValor>
    {
        public async Task<NotaTipoValor> Handle(ObterNotaTipoPorAnoModalidadeDataReferenciaQuery request, CancellationToken cancellationToken)
        {
            return await Task.Run(() => new NotaTipoValor() { TipoNota = TipoNota.Nota });
        }
    }
}
