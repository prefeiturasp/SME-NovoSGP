using MediatR;
using Microsoft.Extensions.Configuration;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarAulaRecorrenteUseCase : AbstractUseCase, IAlterarAulaRecorrenteUseCase
    {
        public AlterarAulaRecorrenteUseCase(IMediator mediator): base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            SentrySdk.AddBreadcrumb($"Mensagem AlterarAulaRecorrenteUseCase", "Rabbit - AlterarAulaRecorrenteUseCase");

            AlterarAulaRecorrenteCommand command = mensagemRabbit.ObterObjetoMensagem<AlterarAulaRecorrenteCommand>();

            return await mediator.Send(command);
        }
    }
}
