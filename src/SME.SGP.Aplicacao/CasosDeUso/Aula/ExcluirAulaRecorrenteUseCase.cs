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
    public class ExcluirAulaRecorrenteUseCase : AbstractUseCase, IExcluirAulaRecorrenteUseCase
    {
        public ExcluirAulaRecorrenteUseCase(IMediator mediator): base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExcluirAulaRecorrenteUseCase", "Rabbit - ExcluirAulaRecorrenteUseCase");

            var command = mensagemRabbit.ObterObjetoFiltro<ExcluirAulaRecorrenteCommand>();

            return await mediator.Send(command);
        }
    }
}
