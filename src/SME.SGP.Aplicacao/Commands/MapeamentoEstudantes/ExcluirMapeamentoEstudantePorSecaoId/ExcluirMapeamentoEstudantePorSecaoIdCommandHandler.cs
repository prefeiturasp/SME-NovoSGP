using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirMapeamentoEstudantePorSecaoIdCommandHandler : IRequestHandler<ExcluirMapeamentoEstudantePorSecaoIdCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioQuestaoMapeamentoEstudante repositorioQuestao { get; }

        public ExcluirMapeamentoEstudantePorSecaoIdCommandHandler(IMediator mediator, IRepositorioQuestaoMapeamentoEstudante repositorioQuestao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<bool> Handle(ExcluirMapeamentoEstudantePorSecaoIdCommand request, CancellationToken cancellationToken)
        {
            var questoesIds = await repositorioQuestao.ObterQuestoesPorSecaoId(request.MapeamentoEstudanteSecaoId);
            foreach (var questaoId in questoesIds)
                await mediator.Send(new ExcluirRespostaRegistroAcaoPorQuestaoIdCommand(questaoId));
            await repositorioQuestao.RemoverLogico(request.MapeamentoEstudanteSecaoId, "mapeamento_estudante_secao_id");
            return true;
        }
    }
}
