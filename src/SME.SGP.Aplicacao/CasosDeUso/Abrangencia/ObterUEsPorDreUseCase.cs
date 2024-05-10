using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUEsPorDreUseCase : AbstractUseCase, IObterUEsPorDreUseCase
    {
        public ObterUEsPorDreUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AbrangenciaUeRetorno>> Executar(UEsPorDreDto dto)
        {
            var login = await mediator.Send(ObterLoginAtualQuery.Instance);
            var perfil = await mediator.Send(ObterPerfilAtualQuery.Instance);

            return await mediator.Send(new ObterUEsPorDREQuery(dto, login, perfil));
        }
    }
}
