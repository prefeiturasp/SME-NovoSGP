using MediatR;
using SME.SGP.Aplicacao.Queries.ComponentesCurriculares.ObterComponentesCurricularesPorAnosEModalidade;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorAnoEscolarUseCase : IObterComponentesCurricularesPorAnoEscolarUseCase
    {
        private readonly IMediator mediator;

        public ObterComponentesCurricularesPorAnoEscolarUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<ComponenteCurricularEol>> Executar(string codigoUe, Modalidade modalidade, int anoLetivo, string[] anosEscolares)
        {
            return await mediator.Send(new ObterComponentesCurricularesPorAnosEModalidadeQuery(codigoUe, modalidade, anosEscolares, anoLetivo));
        }
    }
}
