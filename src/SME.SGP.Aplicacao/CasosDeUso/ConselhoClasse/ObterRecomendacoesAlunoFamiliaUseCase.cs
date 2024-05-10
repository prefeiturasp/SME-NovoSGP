using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
