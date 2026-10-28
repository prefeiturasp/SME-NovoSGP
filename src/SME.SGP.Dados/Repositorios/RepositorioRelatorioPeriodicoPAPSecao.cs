using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioPeriodicoPAPSecao : RepositorioBase<RelatorioPeriodicoPAPSecao>, IRepositorioRelatorioPeriodicoPAPSecao
    {
        public RepositorioRelatorioPeriodicoPAPSecao(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<RelatorioPeriodicoPAPSecao>ObterSecoesComQuestoes(long id)
        {
            const string query = @"
                SELECT
                    -- RelatorioPeriodicoPAPSecao
                    rpps.id AS Id,
                    rpps.criado_em AS CriadoEm,
                    rpps.criado_por AS CriadoPor,
                    rpps.alterado_em AS AlteradoEm,
                    rpps.alterado_por AS AlteradoPor,
                    rpps.alterado_rf AS AlteradoRF,
                    rpps.criado_rf AS CriadoRF,
                    rpps.relatorio_periodico_pap_aluno_id
                        AS RelatorioPeriodicoAlunoId,
                    rpps.secao_relatorio_periodico_pap_id
                        AS SecaoRelatorioPeriodicoId,
                    rpps.concluido AS Concluido,
                    rpps.excluido AS Excluido,

                    -- marcador de início da resposta
                    rppr.id AS RespostaInicio,

                    -- RelatorioPeriodicoPAPResposta
                    rppr.id AS Id,
                    rppr.criado_em AS CriadoEm,
                    rppr.criado_por AS CriadoPor,
                    rppr.alterado_em AS AlteradoEm,
                    rppr.alterado_por AS AlteradoPor,
                    rppr.alterado_rf AS AlteradoRF,
                    rppr.criado_rf AS CriadoRF,
                    rppr.relatorio_periodico_pap_questao_id
                        AS RelatorioPeriodicoQuestaoId,
                    rppr.resposta_id AS RespostaId,
                    rppr.arquivo_id AS ArquivoId,
                    rppr.texto AS Texto,
                    rppr.excluido AS Excluido,

                    -- marcador de início da questão do relatório
                    rppq.id AS QuestaoRelatorioInicio,

                    -- RelatorioPeriodicoPAPQuestao
                    rppq.id AS Id,
                    rppq.criado_em AS CriadoEm,
                    rppq.criado_por AS CriadoPor,
                    rppq.alterado_em AS AlteradoEm,
                    rppq.alterado_por AS AlteradoPor,
                    rppq.alterado_rf AS AlteradoRF,
                    rppq.criado_rf AS CriadoRF,
                    rppq.relatorio_periodico_pap_secao_id
                        AS RelatorioPeriodiocoSecaoId,
                    rppq.questao_id AS QuestaoId,
                    rppq.excluido AS Excluido,

                    -- marcador de início da Questao
                    q.id AS QuestaoInicio,

                    -- Questao
                    q.id AS Id,
                    q.criado_em AS CriadoEm,
                    q.criado_por AS CriadoPor,
                    q.alterado_em AS AlteradoEm,
                    q.alterado_por AS AlteradoPor,
                    q.alterado_rf AS AlteradoRF,
                    q.criado_rf AS CriadoRF,
                    q.questionario_id AS QuestionarioId,
                    q.ordem AS Ordem,
                    q.nome AS Nome,
                    q.observacao AS Observacao,
                    q.obrigatorio AS Obrigatorio,
                    q.tipo AS Tipo,
                    q.opcionais AS Opcionais,
                    q.somente_leitura AS SomenteLeitura,
                    q.dimensao AS Dimensao,
                    q.tamanho AS Tamanho,
                    q.mascara AS Mascara,
                    q.placeholder AS PlaceHolder,
                    q.nome_componente AS NomeComponente,

                    -- marcador de início da Opção de resposta
                    op.id AS OpcaoRespostaInicio,

                    -- OpcaoResposta
                    op.id AS Id,
                    op.criado_em AS CriadoEm,
                    op.criado_por AS CriadoPor,
                    op.alterado_em AS AlteradoEm,
                    op.alterado_por AS AlteradoPor,
                    op.alterado_rf AS AlteradoRF,
                    op.criado_rf AS CriadoRF,
                    op.questao_id AS QuestaoId,
                    op.ordem AS Ordem,
                    op.nome AS Nome,
                    op.observacao AS Observacao,
                    op.excluido AS Excluido

                FROM relatorio_periodico_pap_secao rpps
                INNER JOIN relatorio_periodico_pap_questao rppq
                    ON rppq.relatorio_periodico_pap_secao_id = rpps.id
                   AND NOT rppq.excluido
                INNER JOIN questao q
                    ON q.id = rppq.questao_id
                   AND NOT q.excluido
                INNER JOIN relatorio_periodico_pap_resposta rppr
                    ON rppr.relatorio_periodico_pap_questao_id = rppq.id
                   AND NOT rppr.excluido
                LEFT JOIN opcao_resposta op
                    ON op.id = rppr.resposta_id
                   AND NOT op.excluido
                WHERE rpps.id = @id
                  AND NOT rpps.excluido;";

            var relatorioSecao =
                new RelatorioPeriodicoPAPSecao();

            await database.Conexao.QueryAsync<
                RelatorioPeriodicoPAPSecao,
                RelatorioPeriodicoPAPResposta,
                RelatorioPeriodicoPAPQuestao,
                Questao,
                OpcaoResposta,
                RelatorioPeriodicoPAPSecao>(
                query,
                (secao,
                 relatorioResposta,
                 relatorioQuestao,
                 questao,
                 opcaoResposta) =>
                {
                    if (relatorioSecao.Id == 0)
                    {
                        relatorioSecao = secao;
                    }

                    var questaoPAP =
                        relatorioSecao.Questoes
                            .FirstOrDefault(
                                c => c.Id == relatorioQuestao.Id);

                    if (questaoPAP.EhNulo())
                    {
                        questaoPAP = relatorioQuestao;
                        questaoPAP.Questao = questao;

                        relatorioSecao.Questoes
                            .Add(questaoPAP);
                    }

                    var resposta =
                        questaoPAP.Respostas
                            .FirstOrDefault(
                                c => c.Id == relatorioResposta.Id);

                    if (resposta.NaoEhNulo())
                    {
                        return relatorioSecao;
                    }

                    resposta = relatorioResposta;
                    resposta.Resposta = opcaoResposta;

                    questaoPAP.Respostas.Add(resposta);

                    return relatorioSecao;
                },
                new { id },
                splitOn:
                    "RespostaInicio," +
                    "QuestaoRelatorioInicio," +
                    "QuestaoInicio," +
                    "OpcaoRespostaInicio");

            return relatorioSecao;
        }
    }
}
