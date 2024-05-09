using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class PersistirRelatorioAlunoCommandHandler : IRequestHandler<PersistirRelatorioAlunoCommand,RelatorioPeriodicoPAPAluno>
    {
        private readonly IMediator mediator;

        public PersistirRelatorioAlunoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<RelatorioPeriodicoPAPAluno> Handle(PersistirRelatorioAlunoCommand request, CancellationToken cancellationToken)
        {
            if (request.RelatorioPAPDto.PAPAlunoId.HasValue)
                return await mediator.Send(new ObterRelatorioPeriodicoAlunoPAPQuery(request.RelatorioPAPDto.PAPAlunoId.Value), cancellationToken);

            return await mediator.Send(new SalvarRelatorioPeriodicoAlunoPAPCommand(request.RelatorioPAPDto.AlunoCodigo, 
                request.RelatorioPAPDto.AlunoNome,
                request.RelatorioTurmaId), cancellationToken);
        }
    }
}