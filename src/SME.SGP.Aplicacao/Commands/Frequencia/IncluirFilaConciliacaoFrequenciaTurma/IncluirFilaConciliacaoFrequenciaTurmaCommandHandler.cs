﻿using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaConciliacaoFrequenciaTurmaCommandHandler : IRequestHandler<IncluirFilaConciliacaoFrequenciaTurmaCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaConciliacaoFrequenciaTurmaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaConciliacaoFrequenciaTurmaCommand request, CancellationToken cancellationToken)
        {
            var command = new ValidaAusenciaParaConciliacaoFrequenciaTurmaCommand(request.TurmaCodigo, request.DataInicio, request.DataFim, request.ComponenteCurricularId);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaValidacaoAusenciaConciliacaoFrequenciaTurma, command, Guid.NewGuid(), null));

            return true;
        }
    }
}
