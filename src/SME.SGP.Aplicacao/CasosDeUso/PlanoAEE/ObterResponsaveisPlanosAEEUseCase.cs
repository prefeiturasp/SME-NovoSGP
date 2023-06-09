using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisPlanosAEEUseCase : AbstractUseCase, IObterResponsaveisPlanosAEEUseCase
    {
        public ObterResponsaveisPlanosAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Executar(FiltroPlanosAEEDto param)
        {
            return await mediator.Send(new ObterResponsaveisPlanoAEEQuery(param));
        }
    }
}
