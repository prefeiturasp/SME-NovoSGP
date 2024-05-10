﻿using MediatR;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronismoComponentesCurricularesUseCase : AbstractUseCase, IExecutarSincronismoComponentesCurricularesUseCase
    {
        public ExecutarSincronismoComponentesCurricularesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var componentesEol = await mediator.Send(ObterComponentesCurricularesEolQuery.Instance);
            var componentesSGP = await mediator.Send(ObterComponentesCurricularesQuery.Instance);
            var naoExiste = componentesEol.Where(c => !componentesSGP.Any(x => x.Codigo == c.Codigo));

            if (naoExiste.Any())
                await mediator.Send(new InserirVariosComponentesCurricularesCommand(naoExiste));

            return true;
        }
    }
}
