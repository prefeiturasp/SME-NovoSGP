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
    }
}
