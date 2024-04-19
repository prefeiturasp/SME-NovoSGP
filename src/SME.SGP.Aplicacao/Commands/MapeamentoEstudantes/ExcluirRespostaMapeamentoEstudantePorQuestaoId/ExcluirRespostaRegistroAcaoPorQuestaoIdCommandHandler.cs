using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRespostaMapeamentoEstudantePorQuestaoIdCommandHandler : IRequestHandler<ExcluirRespostaMapeamentoEstudantePorQuestaoIdCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioRespostaMapeamentoEstudante repositorioResposta { get; }

        public ExcluirRespostaMapeamentoEstudantePorQuestaoIdCommandHandler(IMediator mediator, IRepositorioRespostaMapeamentoEstudante repositorioResposta)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioResposta = repositorioResposta ?? throw new ArgumentNullException(nameof(repositorioResposta));
        }

        public async Task<bool> Handle(ExcluirRespostaMapeamentoEstudantePorQuestaoIdCommand request, CancellationToken cancellationToken)
        {
            var respostas = await repositorioResposta.ObterPorQuestaoMapeamentoEstudanteId(request.QuestaoMapeamentoEstudanteId);
            foreach(var resposta in respostas)
                await mediator.Send(new ExcluirRespostaMapeamentoEstudanteCommand(resposta));
            return true;
        }
    }
}
