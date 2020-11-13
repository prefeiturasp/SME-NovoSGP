using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.Anos;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.EscolaAqui.Anos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Anos
{
    public class ObterAnosPorCodigoUeModalidadeEscolaAquiUseCase : IObterAnosPorCodigoUeModalidadeEscolaAquiUseCase
    {
        private readonly IMediator mediator;

        public ObterAnosPorCodigoUeModalidadeEscolaAquiUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AnosPorCodigoUeModalidadeEscolaAquiResult>> Executar(string codigoUe, Modalidade modalidade)
        {
            return await mediator.Send(new ObterAnosPorCodigoUeModalidadeQuery(codigoUe, modalidade));
        }
    }
}
