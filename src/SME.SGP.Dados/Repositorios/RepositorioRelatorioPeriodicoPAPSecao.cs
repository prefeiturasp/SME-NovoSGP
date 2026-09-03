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

        public async Task<RelatorioPeriodicoPAPSecao> ObterSecoesComQuestoes(long id)
        {
            var relatorioSecao = new RelatorioPeriodicoPAPSecao();
            var query = @"select 
                        rpps.id, rpps.relatorio_periodico_pap_aluno_id, rpps.secao_relatorio_periodico_pap_id, rpps.criado_em, rpps.criado_por, rpps.criado_rf,
                        rppr.id, rppr.relatorio_periodico_pap_questao_id, rppr.resposta_id, rppr.arquivo_id, rppr.texto, rppr.excluido, rppr.criado_em, rppr.criado_por, rppr.criado_rf,
                        rppq.id, rppq.relatorio_periodico_pap_secao_id, rppq.questao_id, rppq.excluido,
                        q.id, q.questionario_id, q.ordem, q.nome, q.observacao, q.obrigatorio, q.tipo, q.opcionais, q.somente_leitura,
                        q.dimensao, q.tamanho, q.mascara, q.placeholder, q.nome_componente,
                        op.id, op.questao_id, op.ordem, op.nome, op.observacao
                        from relatorio_periodico_pap_secao rpps
                        inner join relatorio_periodico_pap_questao rppq on rppq.relatorio_periodico_pap_secao_id = rpps.id
                        inner join questao q on q.id = rppq.questao_id and not q.excluido
                        inner join relatorio_periodico_pap_resposta rppr on rppr.relatorio_periodico_pap_questao_id = rppq.id
                        left join opcao_resposta op on op.id = rppr.resposta_id and not op.excluido
                        where rpps.id = @id
                           and not rpps.excluido    
                           and not rppq.excluido 
                           and not rppr.excluido";

            await database.Conexao.QueryAsync<RelatorioPeriodicoPAPSecao, 
                                              RelatorioPeriodicoPAPResposta, 
                                              RelatorioPeriodicoPAPQuestao,
                                              Questao, 
                                              OpcaoResposta,
                                              RelatorioPeriodicoPAPSecao>(query,
                (secao, relatorioResposta, relatorioQuestao, questao, opcaoResposta) =>
                {
                    if (relatorioSecao.Id == 0)
                        relatorioSecao = secao;

                    var questaoPAP = relatorioSecao.Questoes.FirstOrDefault(c => c.Id == relatorioQuestao.Id);

                    if (questaoPAP.EhNulo())
                    {
                        questaoPAP = relatorioQuestao;
                        questaoPAP.Questao = questao;
                        relatorioSecao.Questoes.Add(questaoPAP);
                    }

                    var resposta = questaoPAP.Respostas.FirstOrDefault(c => c.Id == relatorioResposta.Id);

                    if (resposta.NaoEhNulo())
                        return secao;

                    resposta = relatorioResposta;
                    resposta.Resposta = opcaoResposta;
                    questaoPAP.Respostas.Add(resposta);

                    return secao;
                }, new { id });

            if (relatorioSecao.Id == 0)
            {
                const string querySecao = @"select id,
                                                   relatorio_periodico_pap_aluno_id RelatorioPeriodicoAlunoId,
                                                   secao_relatorio_periodico_pap_id SecaoRelatorioPeriodicoId,
                                                   concluido,
                                                   excluido,
                                                   criado_em CriadoEm,
                                                   criado_por CriadoPor,
                                                   criado_rf CriadoRF,
                                                   alterado_em AlteradoEm,
                                                   alterado_por AlteradoPor,
                                                   alterado_rf AlteradoRF
                                              from relatorio_periodico_pap_secao
                                             where id = @id
                                               and not excluido";

                relatorioSecao = await database.Conexao
                    .QueryFirstOrDefaultAsync<RelatorioPeriodicoPAPSecao>(querySecao, new { id });
            }

            return relatorioSecao ?? new RelatorioPeriodicoPAPSecao();
        }

        public Task<long?> ObterIdSecaoAtiva(long relatorioAlunoId, long secaoRelatorioPeriodicoId)
        {
            const string query = @"select rpps.id
                                     from relatorio_periodico_pap_secao rpps
                                    where rpps.relatorio_periodico_pap_aluno_id = @relatorioAlunoId
                                      and rpps.secao_relatorio_periodico_pap_id = @secaoRelatorioPeriodicoId
                                      and not rpps.excluido
                                    order by exists (
                                                 select 1
                                                   from relatorio_periodico_pap_questao rppq
                                                   join relatorio_periodico_pap_resposta rppr
                                                     on rppr.relatorio_periodico_pap_questao_id = rppq.id
                                                    and not rppr.excluido
                                                  where rppq.relatorio_periodico_pap_secao_id = rpps.id
                                                    and not rppq.excluido
                                             ) desc,
                                             coalesce(rpps.alterado_em, rpps.criado_em) desc,
                                             rpps.id desc
                                    limit 1";

            return database.Conexao.QueryFirstOrDefaultAsync<long?>(query,
                new { relatorioAlunoId, secaoRelatorioPeriodicoId });
        }
    }
}
