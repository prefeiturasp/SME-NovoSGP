using MediatR;
using Newtonsoft.Json;
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
    public class ObterRespostasAtualizadasQuestionarioMapeamentoEstudanteQueryHandler : IRequestHandler<ObterRespostasAtualizadasQuestionarioMapeamentoEstudanteQuery, IEnumerable<RespostaQuestaoMapeamentoEstudanteDto>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioQuestaoMapeamentoEstudante repositorioQuestao;
        private readonly IRepositorioRespostaMapeamentoEstudante repositorioResposta;

        public const string PARECER_CONCLUSIVO_ANO_ANTERIOR = "PARECER_CONCLUSIVO_ANO_ANTERIOR";
        public const string TURMA_ANO_ANTERIOR = "TURMA_ANO_ANTERIOR";
        public const string ANOTACOES_PEDAG_BIMESTRE_ANTERIOR = "ANOTACOES_PEDAG_BIMESTRE_ANTERIOR";
        public const string ACOMPANHADO_SRM_CEFAI = "ACOMPANHADO_SRM_CEFAI";
        public const string POSSUI_PLANO_AEE = "POSSUI_PLANO_AEE";
        public const string ACOMPANHADO_NAAPA = "ACOMPANHADO_NAAPA";
        public const string PARTICIPA_PAP = "PARTICIPA_PAP";
        public const string PARTICIPA_MAIS_EDUCACAO = "PARTICIPA_MAIS_EDUCACAO";
        public const string PROJETO_FORTALECIMENTO_APRENDIZAGENS = "PROJETO_FORTALECIMENTO_APRENDIZAGENS";
        public const string QDADE_REGISTROS_BUSCA_ATIVA = "QDADE_REGISTROS_BUSCA_ATIVA";
        public const string MIGRANTE = "MIGRANTE";
        public const string PROGRAMA_SAO_PAULO_INTEGRAL = "PROGRAMA_SAO_PAULO_INTEGRAL";
        public const string FREQUENCIA = "FREQUENCIA";
        public const string HIPOTESE_ESCRITA = "HIPOTESE_ESCRITA";
        public const string AVALIACOES_EXTERNAS_PROVA_SP = "AVALIACOES_EXTERNAS_PROVA_SP";
        

        public ObterRespostasAtualizadasQuestionarioMapeamentoEstudanteQueryHandler(IMediator mediator,         
                                                                                    IRepositorioQuestaoMapeamentoEstudante repositorioQuestao,
                                                                                    IRepositorioRespostaMapeamentoEstudante repositorioResposta)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
            this.repositorioResposta = repositorioResposta ?? throw new ArgumentNullException(nameof(repositorioResposta));
        }

        public async Task<IEnumerable<RespostaQuestaoMapeamentoEstudanteDto>> Handle(ObterRespostasAtualizadasQuestionarioMapeamentoEstudanteQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));
            var informacoesSGP = await mediator.Send(new ObterInformacoesAtualizadasAlunoMapeamentoEstudanteQuery(request.CodigoAluno, turma.AnoLetivo, request.Bimestre));
            var informacoesTurmasPrograma = await mediator.Send(new ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQuery(request.CodigoAluno, turma.AnoLetivo));

            var retorno = new List<RespostaQuestaoMapeamentoEstudanteDto>();
            var questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, PARECER_CONCLUSIVO_ANO_ANTERIOR);
            if (informacoesSGP.IdParecerConclusivoAnoAnterior.HasValue)
                retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
                {
                    QuestaoId = questao,
                    Texto = JsonConvert.SerializeObject(new { index = informacoesSGP.IdParecerConclusivoAnoAnterior, 
                                                              value = informacoesSGP.DescricaoParecerConclusivoAnoAnterior })
                });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, TURMA_ANO_ANTERIOR);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
                {
                    QuestaoId = questao,
                    Texto = informacoesSGP.TurmaAnoAnterior
                });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, ANOTACOES_PEDAG_BIMESTRE_ANTERIOR);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                Texto = informacoesSGP.AnotacoesPedagogicasBimestreAnterior
            });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, ACOMPANHADO_SRM_CEFAI);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                Texto = informacoesTurmasPrograma.ComponentesSRMCEFAI.PossuiRegistros()
                        ? (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(ACOMPANHADO_SRM_CEFAI, "Sim")).ToString() 
                        : (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(ACOMPANHADO_SRM_CEFAI, "Não")).ToString()
            });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, POSSUI_PLANO_AEE);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                Texto = informacoesSGP.PossuiPlanoAEE()
                        ? (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(POSSUI_PLANO_AEE, "Sim")).ToString()
                        : (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(POSSUI_PLANO_AEE, "Não")).ToString()
            });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, ACOMPANHADO_NAAPA);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                Texto = informacoesSGP.AcompanhadoPeloNAAPA()
                        ? (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(ACOMPANHADO_NAAPA, "Sim")).ToString()
                        : (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(ACOMPANHADO_NAAPA, "Não")).ToString()
            });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, PARTICIPA_PAP);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                Texto = informacoesTurmasPrograma.ComponentesPAP.SerializarJsonTipoQuestaoComboMultiplaEscolhaDinamico()
            });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, PARTICIPA_MAIS_EDUCACAO);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                Texto = informacoesTurmasPrograma.ComponentesMaisEducacao.SerializarJsonTipoQuestaoComboMultiplaEscolhaDinamico()
            });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, PROJETO_FORTALECIMENTO_APRENDIZAGENS);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                Texto = informacoesTurmasPrograma.ComponentesFortalecimentoAprendizagens.SerializarJsonTipoQuestaoComboMultiplaEscolhaDinamico()
            });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, QDADE_REGISTROS_BUSCA_ATIVA);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                Texto = informacoesSGP.QdadeBuscasAtivasBimestre.ToString()
            });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, PROGRAMA_SAO_PAULO_INTEGRAL);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                Texto = (TipoTurnoEOL)turma.TipoTurno == TipoTurnoEOL.Integral
                        ? (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(PROGRAMA_SAO_PAULO_INTEGRAL, "Sim")).ToString()
                        : (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(PROGRAMA_SAO_PAULO_INTEGRAL, "Não")).ToString()
            }); ;

            var dadosEstudante = await mediator.Send(new ObterAlunoEnderecoEolQuery(request.CodigoAluno));
            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, MIGRANTE);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                Texto = dadosEstudante.EhImigrante
                        ? (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(MIGRANTE, "Sim")).ToString()
                        : (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(MIGRANTE, "Não")).ToString()
            });

            var freqGeralEstudante = await mediator.Send(new ObterConsultaFrequenciaGeralAlunoQuery(request.CodigoAluno, turma.CodigoTurma));
            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, FREQUENCIA);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                Texto = double.Parse(freqGeralEstudante.Replace("%", "").Replace(",", ".")) >= 75
                        ? (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(FREQUENCIA, "Frequente")).ToString()
                        : (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(FREQUENCIA, "Não Frequente")).ToString()
            });

            return retorno;
        }

    }
}
