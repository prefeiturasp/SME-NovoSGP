using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra.Dtos.EscolaAqui.Anos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosPorCodigoUeModalidadeEscolaAquiUseCase : AbstractUseCase, IObterAnosPorCodigoUeModalidadeEscolaAquiUseCase
    {
        public ObterAnosPorCodigoUeModalidadeEscolaAquiUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AnosPorCodigoUeModalidadeEscolaAquiResult>> Executar(string codigoUe, int[] modalidades)
            => await mediator.Send(new ObterAnosPorCodigoUeModalidadeQuery(codigoUe, modalidades));
    }
}
