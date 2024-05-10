using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtividadesInfantilUseCase : AbstractUseCase, IObterAtividadesInfantilUseCase
    {
        public ObterAtividadesInfantilUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AtividadeInfantilDto>> Executar(long aulaId)
        {
            return await mediator.Send(new ObterAtividadesInfantilPorAulaIdQuery(aulaId));
        }
    }
}
