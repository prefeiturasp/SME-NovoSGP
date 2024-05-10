using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPareceresConclusivosAnoLetivoModalidadeUseCase : IObterPareceresConclusivosAnoLetivoModalidadeUseCase
    {
        private readonly IMediator mediator;

        public ObterPareceresConclusivosAnoLetivoModalidadeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public Task<IEnumerable<ParecerConclusivoDto>> Executar(int anoLetivo, Modalidade modalidade)
        => mediator.Send(new ObterPareceresConclusivosAnoLetivoModalidadeQuery(anoLetivo, modalidade));
    }
}
