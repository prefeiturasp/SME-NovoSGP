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

            return relatorioSecao;
        }
    }
}
