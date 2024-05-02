using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Constantes;
using SME.SGP.Aplicacao.Queries;
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
            var questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, NomesComponentesMapeamentoEstudante.PARECER_CONCLUSIVO_ANO_ANTERIOR);
            if (informacoesSGP.IdParecerConclusivoAnoAnterior.HasValue)
                retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
                {
                    QuestaoId = questao,
                    Texto = JsonConvert.SerializeObject(new { index = informacoesSGP.IdParecerConclusivoAnoAnterior.ToString(), 
                                                              value = informacoesSGP.DescricaoParecerConclusivoAnoAnterior })
                });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, NomesComponentesMapeamentoEstudante.TURMA_ANO_ANTERIOR);
            if (!string.IsNullOrEmpty(informacoesSGP.TurmaAnoAnterior))
                retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
                    {
                        QuestaoId = questao,
                        Texto = informacoesSGP.TurmaAnoAnterior
                    });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, NomesComponentesMapeamentoEstudante.ANOTACOES_PEDAG_BIMESTRE_ANTERIOR);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                Texto = informacoesSGP.AnotacoesPedagogicasBimestreAnterior
            });

            var dadosEstudante = await mediator.Send(new ObterAlunoEnderecoEolQuery(request.CodigoAluno));
            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, NomesComponentesMapeamentoEstudante.MIGRANTE);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                RespostaId = dadosEstudante.EhImigrante
                             ? (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(NomesComponentesMapeamentoEstudante.MIGRANTE, "Sim"))
                             : (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(NomesComponentesMapeamentoEstudante.MIGRANTE, "Não"))
            });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, NomesComponentesMapeamentoEstudante.ACOMPANHADO_SRM_CEFAI);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                RespostaId = informacoesTurmasPrograma.ComponentesSRMCEFAI.PossuiRegistros()
                             ? (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(NomesComponentesMapeamentoEstudante.ACOMPANHADO_SRM_CEFAI, "Sim")) 
                             : (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(NomesComponentesMapeamentoEstudante.ACOMPANHADO_SRM_CEFAI, "Não"))
            });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, NomesComponentesMapeamentoEstudante.POSSUI_PLANO_AEE);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                RespostaId = informacoesSGP.PossuiPlanoAEE()
                             ? (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(NomesComponentesMapeamentoEstudante.POSSUI_PLANO_AEE, "Sim"))
                             : (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(NomesComponentesMapeamentoEstudante.POSSUI_PLANO_AEE, "Não"))
            });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, NomesComponentesMapeamentoEstudante.ACOMPANHADO_NAAPA);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                RespostaId = informacoesSGP.AcompanhadoPeloNAAPA()
                             ? (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(NomesComponentesMapeamentoEstudante.ACOMPANHADO_NAAPA, "Sim"))
                             : (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(NomesComponentesMapeamentoEstudante.ACOMPANHADO_NAAPA, "Não"))
            });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, NomesComponentesMapeamentoEstudante.PARTICIPA_PAP);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                Texto = informacoesTurmasPrograma.ComponentesPAP.SerializarJsonTipoQuestaoComboMultiplaEscolhaDinamico()
            });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, NomesComponentesMapeamentoEstudante.PARTICIPA_MAIS_EDUCACAO);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                Texto = informacoesTurmasPrograma.ComponentesMaisEducacao.SerializarJsonTipoQuestaoComboMultiplaEscolhaDinamico()
            });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, NomesComponentesMapeamentoEstudante.PROJETO_FORTALECIMENTO_APRENDIZAGENS);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                Texto = informacoesTurmasPrograma.ComponentesFortalecimentoAprendizagens.SerializarJsonTipoQuestaoComboMultiplaEscolhaDinamico()
            });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, NomesComponentesMapeamentoEstudante.PROGRAMA_SAO_PAULO_INTEGRAL);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                RespostaId = (TipoTurnoEOL)turma.TipoTurno == TipoTurnoEOL.Integral
                             ? (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(NomesComponentesMapeamentoEstudante.PROGRAMA_SAO_PAULO_INTEGRAL, "Sim"))
                             : (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(NomesComponentesMapeamentoEstudante.PROGRAMA_SAO_PAULO_INTEGRAL, "Não"))
            });

            var sondagem = await mediator.Send(new ObterSondagemLPAlunoQuery(turma.CodigoTurma, request.CodigoAluno));
            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, NomesComponentesMapeamentoEstudante.HIPOTESE_ESCRITA);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                Texto = sondagem.ObterTextoHipoteseEscrita(request.Bimestre)
            });

            var avaliacoesExternasProvaSP = await mediator.Send(new ObterAvaliacoesExternasProvaSPAlunoQuery(request.CodigoAluno, turma.AnoLetivo-1)); 
            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, NomesComponentesMapeamentoEstudante.AVALIACOES_EXTERNAS_PROVA_SP);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                Texto = avaliacoesExternasProvaSP.ToList().SerializarJsonTipoQuestaoAvaliacoesExternasProvaSP()
            });

            string freqGeralAluno = await mediator.Send(new ObterConsultaFrequenciaGeralAlunoQuery(request.CodigoAluno, turma.CodigoTurma));
            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, NomesComponentesMapeamentoEstudante.FREQUENCIA);
            double freqGeral = 0;
            Double.TryParse(freqGeralAluno.Replace("%", "").Replace(",", ".").Trim(), out freqGeral);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                RespostaId = freqGeral >= 75
                             ? (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(NomesComponentesMapeamentoEstudante.FREQUENCIA, "Frequente"))
                             : (await repositorioResposta.ObterIdOpcaoRespostaPorNomeComponenteQuestao(NomesComponentesMapeamentoEstudante.FREQUENCIA, "Não Frequente"))
            });

            questao = await repositorioQuestao.ObterIdQuestaoPorNomeComponenteQuestionario(request.QuestionarioId, NomesComponentesMapeamentoEstudante.QDADE_REGISTROS_BUSCA_ATIVA);
            retorno.Add(new RespostaQuestaoMapeamentoEstudanteDto()
            {
                QuestaoId = questao,
                Texto = informacoesSGP.QdadeBuscasAtivasBimestre.ToString()
            });        

            return retorno;
        }

    }
}
