using MediatR;
using SME.SGP.Dominio;
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
        private readonly IMediator mediator;
        public ExcluirDiarioBordoDaAulaCommandHandler(IRepositorioDiarioBordo repositorioDiarioBordo, 
                                                      IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao, IMediator mediator)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirDiarioBordoDaAulaCommand request, CancellationToken cancellationToken)
        {
            var diariosBordosIds = (await repositorioDiarioBordo.ObterPorAulaId(request.AulaId)).Select(c => c.Id);
            foreach (var idDiarioBordo in diariosBordosIds)
                await mediator.Send(new ExcluirDiarioBordoCommand(idDiarioBordo));
            return true;
        }
    }
}
