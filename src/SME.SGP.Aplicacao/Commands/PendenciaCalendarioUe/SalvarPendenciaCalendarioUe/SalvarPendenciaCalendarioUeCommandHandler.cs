﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaCalendarioUeCommandHandler : IRequestHandler<SalvarPendenciaCalendarioUeCommand, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaCalendarioUe repositorioPendenciaCalendarioUe;

        public SalvarPendenciaCalendarioUeCommandHandler(IMediator mediator, IRepositorioPendenciaCalendarioUe repositorioPendenciaCalendarioUe)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPendenciaCalendarioUe = repositorioPendenciaCalendarioUe ?? throw new ArgumentNullException(nameof(repositorioPendenciaCalendarioUe));
        }

        public async Task<long> Handle(SalvarPendenciaCalendarioUeCommand request, CancellationToken cancellationToken)
        {
            var pendenciaId = await mediator.Send(new SalvarPendenciaCommand(request.TipoPendencia, request.Descricao, request.Instrucao));

            await mediator.Send(new RelacionaPendenciaUsuarioCommand(ObterPerfisParaPendencia(request.TipoPendencia), request.Ue.CodigoUe, pendenciaId, 0));

            return await repositorioPendenciaCalendarioUe.SalvarAsync(new Dominio.PendenciaCalendarioUe()
            {
                PendenciaId = pendenciaId,
                UeId = request.Ue.Id,
                TipoCalendarioId = request.TipoCalendarioId
            });
        }

        private string[] ObterPerfisParaPendencia(TipoPendencia tipoPendencia)
        {
            switch (tipoPendencia)
            {
                case TipoPendencia.CalendarioLetivoInsuficiente:
                    return new string[] { "CP", "AD", "Diretor", "ADM UE" };
                case TipoPendencia.CadastroEventoPendente:
                    return new string[] { "ADM UE" };
                default:
                    return new string[] { };
            }
        }
    }
}
