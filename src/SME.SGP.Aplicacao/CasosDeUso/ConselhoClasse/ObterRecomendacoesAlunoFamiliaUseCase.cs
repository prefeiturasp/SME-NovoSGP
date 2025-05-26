using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterRecomendacoesAlunoFamiliaUseCase : AbstractUseCase, IObterRecomendacoesAlunoFamiliaUseCase
    {
        public ObterRecomendacoesAlunoFamiliaUseCase(IMediator mediator) : base(mediator)
        {

        }
        public async Task<IEnumerable<RecomendacoesAlunoFamiliaDto>> Executar()
            => await mediator.Send(ObterRecomendacoesAlunoFamiliaQuery.Instance);
    }
}
