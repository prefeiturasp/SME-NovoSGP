using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Dados
{
    public class RepositorioCompensacaoAusenciaAlunoAulaConsulta : RepositorioBase<CompensacaoAusenciaAlunoAula>, IRepositorioCompensacaoAusenciaAlunoAulaConsulta
    {
        public RepositorioCompensacaoAusenciaAlunoAulaConsulta(ISgpContextConsultas database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<CompensacaoAusenciaAlunoAulaDto>> ObterCompensacoesAusenciasAlunoEAulaPorAulaIdTurmaComponenteQuantidade(long aulaId, int quantidade)
        {
            var query = @"select caaa.id, caaa.compensacao_ausencia_aluno_id
                          from registro_frequencia_aluno rfa 
                              join compensacao_ausencia_aluno_aula caaa on caaa.registro_frequencia_aluno_id = rfa.id 
                              join aula a on a.id = rfa.aula_id 
                              join turma t on t.turma_id = a.turma_id 
                          where rfa.aula_id = @aulaId 
                                and caaa.numero_aula > @quantidade 
                                and not caaa.excluido";
            
            return await database.Conexao.QueryAsync<CompensacaoAusenciaAlunoAulaDto>(query, new { aulaId, quantidade });
        }

        public Task<IEnumerable<CompensacaoAusenciaAlunoAula>> ObterPorCompensacaoIdAsync(long compensacaoId)
        {
            var sql = $@"SELECT caaa.id, caaa.compensacao_ausencia_aluno_id, caaa.registro_frequencia_aluno_id, caaa.numero_aula, caaa.data_aula, caaa.criado_em, caaa.criado_por, caaa.alterado_em, caaa.alterado_por, caaa.criado_rf, caaa.alterado_rf, caaa.excluido
                         FROM compensacao_ausencia_aluno caa
                         INNER JOIN compensacao_ausencia_aluno_aula caaa on caaa.compensacao_ausencia_aluno_id = caa.id 
                         WHERE caa.compensacao_ausencia_id = @compensacaoId";

            return database.Conexao.QueryAsync<CompensacaoAusenciaAlunoAula>(sql, new { compensacaoId });
        }

        public Task<IEnumerable<CompensacaoAusenciaAlunoAula>> ObterPorAulaIdAsync(long aulaId, long? numeroAula)
        {
            var sql = $@"SELECT caaa.id, caaa.compensacao_ausencia_aluno_id, caaa.registro_frequencia_aluno_id, caaa.numero_aula, caaa.data_aula, caaa.criado_em, caaa.criado_por, caaa.alterado_em, caaa.alterado_por, caaa.criado_rf, caaa.alterado_rf, caaa.excluido
                         FROM registro_frequencia_aluno rfa
                         JOIN compensacao_ausencia_aluno_aula caaa on rfa.id = caaa.registro_frequencia_aluno_id
                         WHERE rfa.aula_id = @aulaId and not caaa.excluido";

            if (numeroAula.HasValue)
                sql += " and rfa.numero_aula = @numeroAula";

            return database.Conexao.QueryAsync<CompensacaoAusenciaAlunoAula>(sql, new { aulaId, numeroAula = numeroAula.GetValueOrDefault() });
        }

        public Task<IEnumerable<CompensacaoAusenciaAlunoAula>> ObterPorRegistroFrequenciaAlunoIdsAsync(long[] registroFrequenciaAlunoIds)
        {
            var sql = $@"SELECT caaa.id, caaa.compensacao_ausencia_aluno_id, caaa.registro_frequencia_aluno_id, caaa.numero_aula, caaa.data_aula, caaa.criado_em, caaa.criado_por, caaa.alterado_em, caaa.alterado_por, caaa.criado_rf, caaa.alterado_rf, caaa.excluido
                         FROM compensacao_ausencia_aluno_aula caaa
                         WHERE caaa.registro_frequencia_aluno_id = ANY(@registroFrequenciaAlunoIds) and not caaa.excluido";

            return database.Conexao.QueryAsync<CompensacaoAusenciaAlunoAula>(sql, new { registroFrequenciaAlunoIds });
        }
    }
}