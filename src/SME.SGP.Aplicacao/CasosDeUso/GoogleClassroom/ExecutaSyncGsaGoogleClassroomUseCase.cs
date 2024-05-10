﻿using MediatR;
using Microsoft.Extensions.Options;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaSyncGsaGoogleClassroomUseCase : AbstractUseCase, IExecutaSyncGsaGoogleClassroomUseCase
    {
        private readonly GoogleClassroomSyncOptions googleClassroomSyncOptions;

        public ExecutaSyncGsaGoogleClassroomUseCase(IMediator mediator, IOptions<GoogleClassroomSyncOptions> googleClassroomSyncOptions) : base(mediator)
        {
            this.googleClassroomSyncOptions = googleClassroomSyncOptions.Value;
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            if (!googleClassroomSyncOptions.ExecutarGsaSync)
                return false;

            string mensagem = "Mensagem API Google Classroom - Comparativo de Dados";
            return await mediator.Send(new PublicarFilaGoogleClassroomCommand(RotasRabbitSgpGoogleClassroomApi.FilaGsaSync, mensagem));
        }
    }
}