using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommandHandler : IRequestHandler<RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommand, long>
    {
        private readonly IRepositorioRespostaEncaminhamentoAEE repositorioRespostaEncaminhamentoAEE;

        public RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommandHandler(IRepositorioRespostaEncaminhamentoAEE repositorioRespostaEncaminhamentoAEE)
        {
            this.repositorioRespostaEncaminhamentoAEE = repositorioRespostaEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioRespostaEncaminhamentoAEE));
        }

        public async Task<long> Handle(RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommand request, CancellationToken cancellationToken)
        {
            var resposta = MapearParaEntidade(request);
            var id = await repositorioRespostaEncaminhamentoAEE.SalvarAsync(resposta);
            return id;
        }

        private RespostaEncaminhamentoAEE MapearParaEntidade(RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommand request)
            => new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = request.QuestaoId,
                RespostaId = request.RespostaId
            };
    }
}
