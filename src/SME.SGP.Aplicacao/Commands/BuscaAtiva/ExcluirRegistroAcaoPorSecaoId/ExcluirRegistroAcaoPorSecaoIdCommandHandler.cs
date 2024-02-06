using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRegistroAcaoPorSecaoIdCommandHandler : IRequestHandler<ExcluirRegistroAcaoPorSecaoIdCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioQuestaoRegistroAcaoBuscaAtiva repositorioQuestao { get; }

        public ExcluirRegistroAcaoPorSecaoIdCommandHandler(IMediator mediator, IRepositorioQuestaoRegistroAcaoBuscaAtiva repositorioQuestao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<bool> Handle(ExcluirRegistroAcaoPorSecaoIdCommand request, CancellationToken cancellationToken)
        {
            var questoesIds = await repositorioQuestao.ObterQuestoesPorSecaoId(request.RegistroAcaoSecaoId);
            foreach (var questaoId in questoesIds)
                await mediator.Send(new ExcluirRespostaRegistroAcaoPorQuestaoIdCommand(questaoId));
            await repositorioQuestao.RemoverLogico(request.RegistroAcaoSecaoId, "registro_acao_busca_ativa_secao_id");
            return true;
        }
    }
}
