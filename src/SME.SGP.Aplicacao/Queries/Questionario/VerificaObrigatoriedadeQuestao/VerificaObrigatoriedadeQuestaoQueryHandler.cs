using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaObrigatoriedadeQuestaoQueryHandler : IRequestHandler<VerificaObrigatoriedadeQuestaoQuery, bool>
    {
        private readonly IRepositorioQuestao repositorioQuestao;

        public VerificaObrigatoriedadeQuestaoQueryHandler(IRepositorioQuestao repositorioQuestao)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<bool> Handle(VerificaObrigatoriedadeQuestaoQuery request, CancellationToken cancellationToken)
            => await repositorioQuestao.VerificaObrigatoriedade(request.QuestaoId);
    }
}
