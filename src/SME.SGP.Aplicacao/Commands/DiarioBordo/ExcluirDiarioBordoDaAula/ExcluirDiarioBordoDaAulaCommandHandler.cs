using MediatR;
using Org.BouncyCastle.Ocsp;
using SME.SGP.Dados.Repositorios;
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
        private readonly IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao;
        private readonly IMediator mediator;
        private const int kIdUsuarioLogado_Zero = 0;

        public ExcluirDiarioBordoDaAulaCommandHandler(IRepositorioDiarioBordo repositorioDiarioBordo, 
                                                      IRepositorioDiarioBordoObservacaoNotificacao repositorioDiarioBordoObservacaoNotificacao,
                                                      IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao, IMediator mediator)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
            this.repositorioDiarioBordoObservacaoNotificacao = repositorioDiarioBordoObservacaoNotificacao ?? throw new ArgumentNullException(nameof(repositorioDiarioBordoObservacaoNotificacao));
            this.repositorioDiarioBordoObservacao = repositorioDiarioBordoObservacao ?? throw new ArgumentNullException(nameof(repositorioDiarioBordoObservacao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirDiarioBordoDaAulaCommand request, CancellationToken cancellationToken)
        {
            // diario_bordo <- observacao <- notificacao
            var idDiarioBordo = (await repositorioDiarioBordo.RemoverLogico(request.AulaId, "aula_id"));
            if (idDiarioBordo > 0)
            {
                var observacoesDiarioBordo = await repositorioDiarioBordoObservacao.ListarPorDiarioBordoAsync(idDiarioBordo, kIdUsuarioLogado_Zero);
                foreach (var obs in observacoesDiarioBordo)
                    await mediator.Send(new ExcluirObservacaoDiarioBordoCommand(obs.Id));
            }

            return true;
        }
    }
}
