using MediatR;
using Minio.DataModel;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioPAPQueryHandler : IRequestHandler<ObterQuestionarioPAPQuery, IEnumerable<QuestaoDto>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRelatorioPeriodicoPAPResposta repositorio;
        private readonly IRepositorioQuestao repositorioQuestao;

        public ObterQuestionarioPAPQueryHandler(
                                                IMediator mediator, 
                                                IRepositorioRelatorioPeriodicoPAPResposta repositorio, 
                                                IRepositorioQuestao repositorioQuestao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<IEnumerable<QuestaoDto>> Handle(ObterQuestionarioPAPQuery request, CancellationToken cancellationToken)
        {
            var respostasEncaminhamento = request.PAPSecaoId.HasValue ?
                    (await repositorio.ObterRespostas(request.PAPSecaoId.Value)).ToList() :
                    new List<RelatorioPeriodicoPAPResposta>();

            var repostaPorTipo = await ObterRepostaPorTipoQuestao(request);

            if (repostaPorTipo != null)
                respostasEncaminhamento.Add(repostaPorTipo);

            return await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(request.QuestionarioId, questaoId =>
                respostasEncaminhamento.Where(c => c.RelatorioPeriodicoQuestao.QuestaoId == questaoId)
                .Select(respostaEncaminhamento =>
                {
                    return new RespostaQuestaoDto()
                    {
                        Id = respostaEncaminhamento.Id,
                        OpcaoRespostaId = respostaEncaminhamento.RespostaId,
                        Texto = respostaEncaminhamento.Texto,
                        Arquivo = respostaEncaminhamento.Arquivo
                    };
                })));
        }

        private async Task<RelatorioPeriodicoPAPResposta> ObterRepostaPorTipoQuestao(ObterQuestionarioPAPQuery request)
        {
            var idQuestao = await this.repositorioQuestao.ObterIdQuestaoPorTipoQuestaoParaQuestionario(request.QuestionarioId, TipoQuestao.InformacoesFrequenciaTurmaPAP);

            if (idQuestao.HasValue)
            {
                var frequencia = await mediator.Send(new ObterFrequenciaTurmaPAPQuery(request.CodigoTurma, request.CodigoAluno, request.PeriodoRelatorio));

                return new RelatorioPeriodicoPAPResposta()
                {
                    RelatorioPeriodicoQuestao = new RelatorioPeriodicoPAPQuestao()
                    {
                        QuestaoId = idQuestao.Value
                    },
                    Texto = JsonConvert.SerializeObject(frequencia)
                };
            }

            return null;
        }
    }
}
