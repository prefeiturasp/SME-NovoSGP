using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioPeriodicoPAPAluno : RepositorioBase<RelatorioPeriodicoPAPAluno>, IRepositorioRelatorioPeriodicoPAPAluno
    {
        public RepositorioRelatorioPeriodicoPAPAluno(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<string>> ObterCodigosDeAlunosComRelatorioJaPreenchido(long turmaId, long periodoRelatorioPAPId)
        {
            var query = @" select rppa.aluno_codigo 
                            from relatorio_periodico_pap_aluno rppa 
                            inner join relatorio_periodico_pap_turma rppt on rppt.id = rppa.relatorio_periodico_pap_turma_id 
                            inner join periodo_relatorio_pap prp on prp.id = rppt.periodo_relatorio_pap_id 
                            where rppt.turma_id = @turmaId
                              and prp.id = @periodoRelatorioPAPId";

            return await database.Conexao.QueryAsync<string>(query, new { turmaId, periodoRelatorioPAPId });
        }

        public Task<RelatorioPAPAlunoConselhoClasseDto> ObterDadosRelatorioPAPAlunoConselhoClasse(int anoLetivo, string alunoCodigo, int bimestre, ModalidadeTipoCalendario modalidade)
        {
            var query = @" select t.turma_id as TurmaCodigo, 
                                  rppa.aluno_codigo as AlunoCodigo, 
                                  prp.id as PeriodoRelatorioPAPId
                           from relatorio_periodico_pap_turma rppt 
                           inner join relatorio_periodico_pap_aluno rppa on rppa.relatorio_periodico_pap_turma_id = rppt.id
                           inner join periodo_relatorio_pap prp on prp.id = rppt.periodo_relatorio_pap_id  
                           inner join periodo_escolar_relatorio_pap perp on perp.periodo_relatorio_pap_id = prp.id
                           inner join periodo_escolar pe on pe.id = perp.periodo_escolar_id 
                           inner join tipo_calendario tc on tc.id = pe.tipo_calendario_id 
                           inner join turma t on t.id = rppt.turma_id 
                           where tc.ano_letivo = @anoLetivo
                                 and tc.modalidade = @modalidade
                                 and pe.bimestre = @bimestre
                                 and rppa.aluno_codigo = @alunoCodigo
                                and not rppt.excluido 
                                and not rppa.excluido 
                           order by rppt.id desc;";

            return database.Conexao.QueryFirstOrDefaultAsync<RelatorioPAPAlunoConselhoClasseDto>(query, new { anoLetivo, alunoCodigo, bimestre, modalidade = (int)modalidade });
        }
    }
}
