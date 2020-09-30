using MediatR;
using SME.SGP.Aplicacao.Queries.ComponentesCurriculares.ObterComponentesCurricularesPorAnosEModalidade;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarComponentesCurricularesEolUseCase : IListarComponentesCurricularesEolUseCase
    {
        private readonly IMediator mediator;

        public ListarComponentesCurricularesEolUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ComponenteCurricularDto>> Executar()
        {
            return await mediator.Send(new ObterComponentesCurricularesEolQuery());
        }
    }
}
