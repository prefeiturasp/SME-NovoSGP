using MediatR;
using SME.SGP.Aplicacao.Queries.ComponentesCurriculares.ObterComponentesCurricularesPorAnosEModalidade;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SincronizarComponentesCurricularesUseCase : AbstractUseCase, ISincronizarComponentesCurricularesUseCase
    {
        public SincronizarComponentesCurricularesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var componentesEol = await mediator.Send(new ObterComponentesCurricularesEolQuery());
            var componentesSGP = await mediator.Send(new ObterComponentesCurricularesQuery());
            var naoExiste = componentesEol.Where(c => !componentesSGP.Any(x => x.Codigo == c.Codigo));

            if (naoExiste.Any())
                await mediator.Send(new InserirVariosComponentesCurricularesCommand(naoExiste));

            return true;
        }
    }
}
