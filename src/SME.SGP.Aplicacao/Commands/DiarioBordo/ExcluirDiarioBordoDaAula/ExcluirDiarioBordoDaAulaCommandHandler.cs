using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class ExcluirDiarioBordoDaAulaCommandHandler : IRequestHandler<ExcluirDiarioBordoDaAulaCommand, bool>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;
        private readonly IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao;
        private readonly IMediator mediator;
        private const int ID_USUARIO_LOGADO_ZERO = 0;

        public ExcluirDiarioBordoDaAulaCommandHandler(IRepositorioDiarioBordo repositorioDiarioBordo, 
                                                      IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao, IMediator mediator)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
            this.repositorioDiarioBordoObservacao = repositorioDiarioBordoObservacao ?? throw new ArgumentNullException(nameof(repositorioDiarioBordoObservacao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirDiarioBordoDaAulaCommand request, CancellationToken cancellationToken)
        {
            var diariosBordosIds = (await repositorioDiarioBordo.ObterPorAulaId(request.AulaId)).Select(c => c.Id);

            foreach (var idDiarioBordo in diariosBordosIds)
            {
                var observacoesDiarioBordo = await repositorioDiarioBordoObservacao.ListarPorDiarioBordoAsync(idDiarioBordo, ID_USUARIO_LOGADO_ZERO);

                foreach (var obs in observacoesDiarioBordo)
                    await mediator.Send(new ExcluirObservacaoDiarioBordoCommand(obs.Id), cancellationToken);                
            }
            
            // diario_bordo <- observacao <- notificacao
            await repositorioDiarioBordo.RemoverLogico(request.AulaId, "aula_id");            

            return true;
        }
    }
}
