using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirQuestaoRegistroAcaoPorIdCommandHandler : IRequestHandler<ExcluirQuestaoRegistroAcaoPorIdCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioQuestaoRegistroAcaoBuscaAtiva repositorioQuestao { get; }

        public ExcluirQuestaoRegistroAcaoPorIdCommandHandler(IMediator mediator, IRepositorioQuestaoRegistroAcaoBuscaAtiva repositorioQuestao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<bool> Handle(ExcluirQuestaoRegistroAcaoPorIdCommand request, CancellationToken cancellationToken)
        {
            await repositorioQuestao.RemoverLogico(request.QuestaoId);
            await mediator.Send(new ExcluirRespostaRegistroAcaoPorQuestaoIdCommand(request.QuestaoId));

            return true;
        }
    }
}
