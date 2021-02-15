using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class SalvarPlanoAeeCommandHandler : IRequestHandler<SalvarPlanoAeeCommand, RetornoPlanoAEEDto>
    {

        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;
        private readonly IRepositorioPlanoAEEVersao repositorioPlanoAEEVersao;
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public SalvarPlanoAeeCommandHandler(
            IRepositorioPlanoAEE repositorioPlanoAEE,
            IRepositorioPlanoAEEVersao repositorioPlanoAEEVersao,
            IMediator mediator,
            IUnitOfWork unitOfWork)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.repositorioPlanoAEEVersao = repositorioPlanoAEEVersao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEVersao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<RetornoPlanoAEEDto> Handle(SalvarPlanoAeeCommand request, CancellationToken cancellationToken)
        {
            var plano = MapearParaEntidade(request);

            var planoAeeDto = request.PlanoAEEDto;
            var planoId = planoAeeDto.Id.GetValueOrDefault();

            // Última versão plano
            int ultimaVersaoPlanoAee = planoId != 0 ? await ObterUltimaVersaoPlanoAEE(planoId) : 1;

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    // Salva Plano
                    if (planoId == 0)
                        planoId = await repositorioPlanoAEE.SalvarAsync(plano);

                    // Salva Versao
                    var planoAEEVersaoId = await SalvarPlanoAEEVersao(planoId, ultimaVersaoPlanoAee);


                    // Salva Questoes
                    foreach (var questao in planoAeeDto.Questoes)
                    {
                        var planoAEEQuestaoId = await mediator.Send(new SalvarPlanoAEEQuestaoCommand(planoId, questao.QuestaoId, planoAEEVersaoId));

                        await mediator.Send(new SalvarPlanoAEERespostaCommand(planoAEEQuestaoId, questao.Resposta, questao.TipoQuestao));
                    }

                    transacao.Commit();
                    return new RetornoPlanoAEEDto(planoId, planoAEEVersaoId);
                }
                catch (Exception ex)
                {
                    transacao.Rollback();
                    throw ex;
                }
            }
        }

        private async Task<long> SalvarPlanoAEEVersao(long planoId, int ultimaVersaoPlanoAee)
        {
            var planoVersaoEntidade = new PlanoAEEVersao
            {
                PlanoAEEId = planoId,
                Numero = ultimaVersaoPlanoAee
            };
            return await repositorioPlanoAEEVersao.SalvarAsync(planoVersaoEntidade);
        }

        private async Task<int> ObterUltimaVersaoPlanoAEE(long planoId)
        {
            var versaoPlano = await repositorioPlanoAEEVersao.ObterUltimaVersaoPorPlanoId(planoId);

            return versaoPlano != null ?
                versaoPlano.Numero + 1 : 1;
        }

        private PlanoAEE MapearParaEntidade(SalvarPlanoAeeCommand request)
            => new PlanoAEE()
            {
                TurmaId = request.TurmaId,
                Situacao = request.Situacao,
                AlunoCodigo = request.AlunoCodigo,
                AlunoNumero = request.AlunoNumero,
                AlunoNome = request.AlunoNome,
                Questoes = new System.Collections.Generic.List<PlanoAEEQuestao>()
            };
    }
}
