﻿using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.FilaRabbit.IncluirFilaInserirAulaRecorrente
{
    public class IncluirFilaInserirAulaRecorrenteCommandHandler : IRequestHandler<IncluirFilaInserirAulaRecorrenteCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaInserirAulaRecorrenteCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaInserirAulaRecorrenteCommand request, CancellationToken cancellationToken)
        {
            var command = new InserirAulaRecorrenteCommand(request.Usuario,
                                                           request.DataAula,
                                                           request.Quantidade,
                                                           request.CodigoTurma,
                                                           request.ComponenteCurricularId,
                                                           request.NomeComponenteCurricular,
                                                           request.TipoCalendarioId,
                                                           request.TipoAula,
                                                           request.CodigoUe,
                                                           request.EhRegencia,
                                                           request.RecorrenciaAula,
                                                           request.CodigoTerritorioSaber);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaInserirAulaRecorrencia, command, Guid.NewGuid(), request.Usuario, true));

            return true;
        }
    }
}
