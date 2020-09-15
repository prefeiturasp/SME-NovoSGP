using MediatR;
using Org.BouncyCastle.Ocsp;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class ExcluirDiarioBordoDaAulaCommandHandler : IRequestHandler<ExcluirDiarioBordoDaAulaCommand, bool>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;
        private readonly IRepositorioDiarioBordoObservacaoNotificacao repositorioDiarioBordoObservacaoNotificacao;
        private readonly IMediator mediator;

        public ExcluirDiarioBordoDaAulaCommandHandler(IRepositorioDiarioBordo repositorioDiarioBordo, IRepositorioDiarioBordoObservacaoNotificacao repositorioDiarioBordoObservacaoNotificacao, IMediator mediator)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
            this.repositorioDiarioBordoObservacaoNotificacao = repositorioDiarioBordoObservacaoNotificacao ?? throw new ArgumentNullException(nameof(repositorioDiarioBordoObservacaoNotificacao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirDiarioBordoDaAulaCommand request, CancellationToken cancellationToken)
        {
            // diario_bordo <- observacao <- notificacao
            var idDiarioBordo = (await repositorioDiarioBordo.RemoverLogico(request.AulaId, "aula_id"));
            if (idDiarioBordo > 0)
            {
                var observacoesId = await repositorioDiarioBordoObservacaoNotificacao.ObterObservacaoPorId(idDiarioBordo);
                foreach (long observacaoId in observacoesId)
                {
                    await mediator.Send(new ExcluirObservacaoDiarioBordoCommand(observacaoId));
                }
            }

            return true;
        }
    }
}
