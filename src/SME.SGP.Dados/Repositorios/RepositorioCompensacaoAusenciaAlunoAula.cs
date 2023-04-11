using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados
{
    public class RepositorioCompensacaoAusenciaAlunoAula : RepositorioBase<CompensacaoAusenciaAlunoAula>, IRepositorioCompensacaoAusenciaAlunoAula
    {
        public RepositorioCompensacaoAusenciaAlunoAula(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }
        
        public async Task<bool> ExclusaoLogicaCompensacaoAusenciaAlunoAulaPorIds(long[] ids)
        {
            var sql = $@"update compensacao_ausencia_aluno_aula set excluido = true where id = ANY(@ids)";

            return await database.Conexao.ExecuteScalarAsync<bool>(sql, new { ids});
        }

        public Task<IEnumerable<CompensacaoAusenciaAlunoAula>> ObterPorCompensacaoIdAsync(long compensacaoId)
        {
            var sql = $@"SELECT caaa.id, caaa.compensacao_ausencia_aluno_id, caaa.registro_frequencia_aluno_id, caaa.numero_aula, caaa.data_aula, caaa.criado_em, caaa.criado_por, caaa.alterado_em, caaa.alterado_por, caaa.criado_rf, caaa.alterado_rf, caaa.excluido
                         FROM compensacao_ausencia_aluno caa
                         INNER JOIN compensacao_ausencia_aluno_aula caaa on caaa.compensacao_ausencia_aluno_id = caa.id 
                         WHERE caa.compensacao_ausencia_id = @compensacaoId";

            return database.Conexao.QueryAsync<CompensacaoAusenciaAlunoAula>(sql, new { compensacaoId });
        }
    }
}
