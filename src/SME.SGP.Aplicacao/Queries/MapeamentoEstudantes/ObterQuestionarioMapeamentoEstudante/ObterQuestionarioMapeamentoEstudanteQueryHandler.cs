using MediatR;
using SME.SGP.Aplicacao.Constantes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioMapeamentoEstudanteQueryHandler : IRequestHandler<ObterQuestionarioMapeamentoEstudanteQuery, IEnumerable<QuestaoDto>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioQuestaoMapeamentoEstudante repositorioQuestao;
        
        public ObterQuestionarioMapeamentoEstudanteQueryHandler(IMediator mediator, IRepositorioQuestaoMapeamentoEstudante repositorioQuestao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<IEnumerable<QuestaoDto>> Handle(ObterQuestionarioMapeamentoEstudanteQuery request, CancellationToken cancellationToken)
        {
            var mapeamentoEstudante = request.MapeamentoEstudanteId.HasValue 
                                        ? await repositorioQuestao.ObterRespostasMapeamentoEstudante(request.MapeamentoEstudanteId.Value)
                                        : await mediator.Send(new ObterRespostasAtualizadasQuestionarioMapeamentoEstudanteQuery(request.QuestionarioId,
                                                                                                                                request.TurmaId.Value,
                                                                                                                                request.CodigoAluno,
                                                                                                                                request.Bimestre.Value));

            var questoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(request.QuestionarioId , questaoId =>
                mapeamentoEstudante.Where(c => c.QuestaoId == questaoId)
                .Select(mapeamento =>
                {
                    return new RespostaQuestaoDto()
                    {
                        Id = mapeamento.Id,
                        OpcaoRespostaId = mapeamento.RespostaId,
                        Texto = mapeamento.Texto,
                        Arquivo = mapeamento.Arquivo
                    };
                })));

            var questao = questoes.FirstOrDefault(q => q.NomeComponente == NomesComponentesMapeamentoEstudante.PARECER_CONCLUSIVO_ANO_ANTERIOR);
            if (questao.Resposta.NaoPossuiRegistros())
                questao.SomenteLeitura = false;

            questao = questoes.FirstOrDefault(q => q.NomeComponente == NomesComponentesMapeamentoEstudante.TURMA_ANO_ANTERIOR);
            if (questao.Resposta.NaoPossuiRegistros())
                questao.SomenteLeitura = false;

            return questoes;
        }

    }
}
