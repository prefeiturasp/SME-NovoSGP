using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRespostaRegistroAcaoPorQuestaoIdCommandHandler : IRequestHandler<ExcluirRespostaRegistroAcaoPorQuestaoIdCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioRespostaRegistroAcaoBuscaAtiva repositorioResposta { get; }

        public ExcluirRespostaRegistroAcaoPorQuestaoIdCommandHandler(IMediator mediator, IRepositorioRespostaRegistroAcaoBuscaAtiva repositorioRespostaEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioResposta = repositorioResposta ?? throw new ArgumentNullException(nameof(repositorioResposta));
        }

        public async Task<bool> Handle(ExcluirRespostaRegistroAcaoPorQuestaoIdCommand request, CancellationToken cancellationToken)
        {
            var respostas = await repositorioResposta.ObterPorQuestaoRegistroAcaoId(request.QuestaoRegistroAcaoId);
            foreach(var resposta in respostas)
                await mediator.Send(new ExcluirRespostaRegistroAcaoCommand(resposta));
            return true;
        }
    }
}
