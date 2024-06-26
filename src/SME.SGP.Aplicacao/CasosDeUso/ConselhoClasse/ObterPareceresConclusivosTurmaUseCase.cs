﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPareceresConclusivosTurmaUseCase : AbstractUseCase, IObterPareceresConclusivosTurmaUseCase
    {
        public ObterPareceresConclusivosTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<ParecerConclusivoDto>> Executar(long turmaId, bool anoLetivoAnterior)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(turmaId));

            if (turma.EhNulo())
                throw new NegocioException("Turma não encontrada.");

            return await mediator.Send(new ObterPareceresConclusivosTurmaQuery(turma, anoLetivoAnterior));
        }
    }
}
