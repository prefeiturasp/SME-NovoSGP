using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class SalvarPlanoAEEQuestaoCommandHandler : IRequestHandler<SalvarPlanoAEEQuestaoCommand, long>
    {
        private readonly IRepositorioPlanoAEEQuestao repositorioPlanoAEEQuestao;

        public SalvarPlanoAEEQuestaoCommandHandler(IRepositorioPlanoAEEQuestao repositorioPlanoAEEQuestao)
        {
            this.repositorioPlanoAEEQuestao = repositorioPlanoAEEQuestao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEQuestao));
        }

        public async Task<long> Handle(SalvarPlanoAEEQuestaoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var planoAEEQuestao = MapearParaEntidade(request);
                return await repositorioPlanoAEEQuestao.SalvarAsync(planoAEEQuestao);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private PlanoAEEQuestao MapearParaEntidade(SalvarPlanoAEEQuestaoCommand request)
            => new PlanoAEEQuestao()
            {
                QuestaoId = request.QuestaoId,
                PlanoAEEVersaoId = request.PlanoVersaoId,
            };
    }
}
