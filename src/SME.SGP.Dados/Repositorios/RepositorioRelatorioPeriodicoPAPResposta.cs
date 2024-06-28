using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioPeriodicoPAPResposta : RepositorioBase<RelatorioPeriodicoPAPResposta>, IRepositorioRelatorioPeriodicoPAPResposta
    {
        public RepositorioRelatorioPeriodicoPAPResposta(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<RelatorioPeriodicoPAPResposta>> ObterRespostas(long papSecaoId)
        {
            var query = @"select rppr.id, rppr.relatorio_periodico_pap_questao_id, rppr.resposta_id, rppr.arquivo_id, rppr.texto, rppr.excluido,
                        rppq.id, rppq.relatorio_periodico_pap_secao_id, rppq.questao_id, rppq.excluido,
                        a.id, a.nome, a.codigo, a.tipo, a.tipo_conteudo
                        from relatorio_periodico_pap_questao rppq
                        inner join relatorio_periodico_pap_resposta rppr on rppr.relatorio_periodico_pap_questao_id = rppq.id
                        left join arquivo a on a.id = rppr.arquivo_id 
                        where rppq.relatorio_periodico_pap_secao_id = @papSecaoId
                           and not rppq.excluido 
                           and not rppr.excluido";

            return await database.Conexao.QueryAsync<RelatorioPeriodicoPAPResposta, RelatorioPeriodicoPAPQuestao, Arquivo, RelatorioPeriodicoPAPResposta>(query,
            (resposta, periodicoPAPQuestao, arquivo) =>
            {
                resposta.RelatorioPeriodicoQuestao = periodicoPAPQuestao;
                resposta.Arquivo = arquivo;

                return resposta;
            }, new { papSecaoId });
        }

        public async Task<IEnumerable<RelatorioPeriodicoPAPResposta>> ObterRespostasPeriodosAnteriores(string codigoAluno, string codigoTurma, long periodoRelatorioId)
        {
            var query = @"select rppr.id, rppr.relatorio_periodico_pap_questao_id, rppr.resposta_id, rppr.arquivo_id, rppr.texto, rppr.excluido,
                        rppq.id, rppq.relatorio_periodico_pap_secao_id, rppq.questao_id, rppq.excluido,
                        a.id, a.nome, a.codigo, a.tipo, a.tipo_conteudo
                        from relatorio_periodico_pap_questao rppq
                        inner join relatorio_periodico_pap_resposta rppr on rppr.relatorio_periodico_pap_questao_id = rppq.id
                        inner join relatorio_periodico_pap_secao rpps on rpps.id = rppq.relatorio_periodico_pap_secao_id 
                        left join arquivo a on a.id = rppr.arquivo_id 
                        where rpps.relatorio_periodico_pap_aluno_id = (select max(rppa.id) 
                                                                        from relatorio_periodico_pap_aluno rppa 
                                                                        inner join relatorio_periodico_pap_turma rppt on rppt.id = rppa.relatorio_periodico_pap_turma_id 
                                                                        inner join periodo_relatorio_pap prp on prp.id = rppt.periodo_relatorio_pap_id 
                                                                        inner join periodo_escolar_relatorio_pap perp on perp.periodo_relatorio_pap_id = prp.id
                                                                        inner join periodo_escolar pe on pe.id = perp.periodo_escolar_id 
                                                                        inner join configuracao_relatorio_pap crp on crp.id = prp.configuracao_relatorio_pap_id 
                                                                        inner join turma t on t.id = rppt.turma_id
                                                                        where crp.inicio_vigencia = (select max(crp_.inicio_vigencia)   
                                                                                                     from relatorio_periodico_pap_aluno rppa_ 
                                                                                                     inner join relatorio_periodico_pap_turma rppt_ on rppt_.id = rppa_.relatorio_periodico_pap_turma_id 
                                                                                                     inner join periodo_relatorio_pap prp_ on prp_.id = rppt_.periodo_relatorio_pap_id 
                                                                                                     inner join configuracao_relatorio_pap crp_ on crp_.id = prp_.configuracao_relatorio_pap_id 
                                                                                                     where rppa_.aluno_codigo = @codigoAluno 
                                                                                                       and rppt_.turma_id = t.id)
                                                                        and pe.bimestre < (select max(bimestre) 
                                                                                            from periodo_escolar_relatorio_pap perp 
                                                                                            inner join periodo_escolar pe on pe.id = perp.periodo_escolar_id 
                                                                                            where perp.periodo_relatorio_pap_id = @periodoRelatorioId)
                                                                        and rppa.aluno_codigo = @codigoAluno 
                                                                        and t.turma_id = @codigoTurma)
                           and not rppq.excluido 
                           and not rppr.excluido";

            return await database.Conexao.QueryAsync<RelatorioPeriodicoPAPResposta, 
                                                     RelatorioPeriodicoPAPQuestao, 
                                                     Arquivo, 
                                                     RelatorioPeriodicoPAPResposta>(query,
                                                                                    (resposta, periodicoPAPQuestao, arquivo) =>
                                                                                    {
                                                                                        resposta.RelatorioPeriodicoQuestao = periodicoPAPQuestao;
                                                                                        resposta.Arquivo = arquivo;

                                                                                        return resposta;
                                                                                    }, new { codigoAluno, codigoTurma, periodoRelatorioId });
        }

        public async Task<bool> RemoverPorArquivoId(long arquivoId)
        {
            var sql =$@"update relatorio_periodico_pap_resposta 
                        set excluido = true,
                            arquivo_id = null
                    where arquivo_id = @arquivoId";

            return (await database.Conexao.ExecuteAsync(sql, new { arquivoId })) > 0;
        }
    }
}
