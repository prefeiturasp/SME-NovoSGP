using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSecaoRelatorioPeriodicoPAP : RepositorioBase<SecaoRelatorioPeriodicoPAP>, IRepositorioSecaoRelatorioPeriodicoPAP
    {
        public RepositorioSecaoRelatorioPeriodicoPAP(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<SecaoTurmaAlunoPAPDto> ObterSecoesPorAluno(string codigoTurma, string codigoAluno, long pAPPeriodoId)
        {
            var secao = new SecaoTurmaAlunoPAPDto();
            var sql = @"select srpp.id, 
                        srpp.nome, srpp.nome_componente NomeComponente, srpp.ordem, srpp.questionario_id QuestionarioId, 
                        case when questao is null then false else true end QuestoesObrigatorias,
                        alunoTurma.PAPSecaoId, 
                        case when alunoTurma.concluido is null then false else true end Concluido,
                        alunoTurma.PAPTurmaId, alunoTurma.PAPAlunoId,
                        alunoTurma.id,
                        alunoTurma.AlteradoEm,
                        alunoTurma.AlteradoPor,
                        alunoTurma.AlteradoRF,
                        alunoTurma.CriadoEm,
                        alunoTurma.CriadoPor,
                        alunoTurma.CriadoRF
                        from secao_config_relatorio_periodico_pap scrpp 
                        inner join secao_relatorio_periodico_pap srpp on scrpp.secao_relatorio_periodico_pap_id = srpp.id 
                        inner join questionario q on q.id = srpp.questionario_id 
                        inner join periodo_relatorio_pap prp on prp.configuracao_relatorio_pap_id = scrpp.configuracao_relatorio_pap_id 
                        left join questao on questao.questionario_id = q.id and questao.obrigatorio = true
                        left join (select  
                                    rpps.concluido,
                                    rpps.id PAPSecaoId, 
                                    rpps.secao_relatorio_periodico_pap_id,
                                    rppt.periodo_relatorio_pap_id,
                                    rppt.id PAPTurmaId, rppa.id PAPAlunoId,
                                    rpps.id,
                                    rpps.Alterado_Em as AlteradoEm,
                                    rpps.Alterado_Por as AlteradoPor,
                                    rpps.Alterado_RF as AlteradoRF,
                                    rpps.Criado_Em as CriadoEm,
                                    rpps.Criado_Por as CriadoPor,
                                    rpps.Criado_RF as CriadoRF
                                    from relatorio_periodico_pap_turma rppt
                                    inner join turma t on t.id = rppt.turma_id 
                                    inner join relatorio_periodico_pap_aluno rppa on rppa.relatorio_periodico_pap_turma_id = rppt.id 
                                    inner join relatorio_periodico_pap_secao rpps on rpps.relatorio_periodico_pap_aluno_id = rppa.id 
                                    where t.turma_id = @codigoTurma and rppa.aluno_codigo = @codigoAluno) 
                        alunoTurma on alunoTurma.periodo_relatorio_pap_id = prp.id and alunoTurma.secao_relatorio_periodico_pap_id = srpp.id
                        where prp.id = @pAPPeriodoId 
                        order by ordem";

            var secoes = await database.Conexao.QueryAsync<SecaoPAPDto, AuditoriaDto, SecaoPAPDto>(
                sql, (secaoPAP, auditoria) =>
                {
                    secaoPAP.Auditoria = auditoria;
                    if (secaoPAP.PAPAlunoId.HasValue)
                        secao.PAPAlunoId = secaoPAP.PAPAlunoId.GetValueOrDefault();
                    if (secaoPAP.PAPTurmaId.HasValue)
                        secao.PAPTurmaId = secaoPAP.PAPTurmaId.GetValueOrDefault();

                    return secaoPAP;
                }, 
                new { codigoTurma, codigoAluno, pAPPeriodoId });

            secao.Secoes.AddRange(secoes);

            return secao;
        }
    }
}
