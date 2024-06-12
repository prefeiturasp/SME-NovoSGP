using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirQuestaoMapeamentoEstudantePorIdCommandHandler : IRequestHandler<ExcluirQuestaoMapeamentoEstudantePorIdCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioQuestaoMapeamentoEstudante repositorioQuestao { get; }

        public ExcluirQuestaoMapeamentoEstudantePorIdCommandHandler(IMediator mediator, IRepositorioQuestaoMapeamentoEstudante repositorioQuestao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<bool> Handle(ExcluirQuestaoMapeamentoEstudantePorIdCommand request, CancellationToken cancellationToken)
        {
            await repositorioQuestao.RemoverLogico(request.QuestaoId);
            await mediator.Send(new ExcluirRespostaMapeamentoEstudantePorQuestaoIdCommand(request.QuestaoId));

            return true;
        }
    }
}
