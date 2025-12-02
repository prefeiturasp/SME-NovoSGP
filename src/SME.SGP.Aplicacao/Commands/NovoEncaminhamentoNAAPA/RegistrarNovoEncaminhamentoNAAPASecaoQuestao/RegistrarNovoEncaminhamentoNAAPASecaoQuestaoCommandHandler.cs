using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.RegistrarNovoEncaminhamentoNAAPASecaoQuestao
{
    public class RegistrarNovoEncaminhamentoNAAPASecaoQuestaoCommandHandler : IRequestHandler<RegistrarNovoEncaminhamentoNAAPASecaoQuestaoCommand, long>
    {
        private readonly IRepositorioQuestaoNovoEncaminhamentoNAAPA repositorioQuestaoNovoEncaminhamentoNAAPA;

        public RegistrarNovoEncaminhamentoNAAPASecaoQuestaoCommandHandler(IRepositorioQuestaoNovoEncaminhamentoNAAPA repositorioQuestaoNovoEncaminhamentoNAAPA)
        {
            this.repositorioQuestaoNovoEncaminhamentoNAAPA = repositorioQuestaoNovoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioQuestaoNovoEncaminhamentoNAAPA));
        }

        public async Task<long> Handle(RegistrarNovoEncaminhamentoNAAPASecaoQuestaoCommand request, CancellationToken cancellationToken)
        {
            var questao = MapearParaEntidade(request);
            return await repositorioQuestaoNovoEncaminhamentoNAAPA.SalvarAsync(questao);
        }

        private QuestaoEncaminhamentoNAAPA MapearParaEntidade(RegistrarNovoEncaminhamentoNAAPASecaoQuestaoCommand request)
            => new QuestaoEncaminhamentoNAAPA()
            {
                QuestaoId = request.QuestaoId,
                EncaminhamentoNAAPASecaoId = request.SecaoId
            };
    }
}