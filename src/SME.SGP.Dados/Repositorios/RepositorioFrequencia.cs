using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequencia : RepositorioBase<RegistroFrequencia>, IRepositorioFrequencia
    {
        public RepositorioFrequencia(ISgpContext database) : base(database)
        {
        }

        public async Task ExcluirFrequenciaAula(long aulaId)
        {
            // Exclui registros de ausencia do aluno
            var command = @"update registro_ausencia_aluno
                                set excluido = true
                            where not excluido
                              and registro_frequencia_id in (
                                select id from registro_frequencia
                                 where not excluido
                                   and aula_id = @aulaId)";
            await database.ExecuteAsync(command, new { aulaId });

            // Exclui registro de frequencia da aula
            command = @"update registro_frequencia
                            set excluido = true
                        where not excluido
                          and aula_id = @aulaId ";
            await database.ExecuteAsync(command, new { aulaId });
        }

        public async Task<bool> FrequenciaAulaRegistrada(long aulaId)
        {
            var query = @"select 1 from registro_frequencia where aula_id = @aulaId";
            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { aulaId });
        }

        public RegistroFrequenciaAulaDto ObterAulaDaFrequencia(long registroFrequenciaId)
        {
            var query = @"select a.ue_id as codigoUe, a.turma_id as codigoTurma
                            , a.disciplina_id as codigoDisciplina, a.data_aula as DataAula
	                        , a.professor_rf as professorRf, t.nome as nomeTurma, ue.nome as nomeUe
                            , ue.dre_id as codigoDre
                         from registro_frequencia rf
                        inner join aula a on a.id = rf.aula_id
                        inner join turma t on t.turma_id = a.turma_id
                        inner join ue on ue.ue_id = a.ue_id
                        where rf.id = @registroFrequenciaId";

            return database.Conexao.QueryFirstOrDefault<RegistroFrequenciaAulaDto>(query, new { registroFrequenciaId });
        }

        public IEnumerable<AulasPorTurmaDisciplinaDto> ObterAulasSemRegistroFrequencia(string turmaId, string disciplinaId)
        {
            var query = @"select a.id, a.professor_rf as professorId, a.data_aula as dataAula, a.quantidade
                          from aula a
                          left join registro_frequencia r on r.aula_id = a.id
                         where not a.excluido
                           and r.id is null
                           and a.data_aula < DATE(now())
                           and a.turma_id = @turmaId
                           and a.disciplina_id = @disciplinaId";

            return database.Conexao.Query<AulasPorTurmaDisciplinaDto>(query, new { turmaId, disciplinaId });
        }

        public async Task<IEnumerable<AusenciaAlunoDto>> ObterAusencias(string turmaCodigo, string disciplinaCodigo, DateTime[] datas, string[] alunoCodigos)
        {
            var query = @"select a.codigo_aluno as AlunoCodigo, a.data_aula as AulaData from (select raa.codigo_aluno, a.quantidade, a.data_aula, count(raa.id) as faltas  from registro_frequencia rf
                            inner join aula a
                            on rf.aula_id = a.id
                            inner join registro_ausencia_aluno raa
                            on raa.registro_frequencia_id = rf.id
                            where  a.turma_id = @turmaCodigo
                            and a.disciplina_id = @disciplinaCodigo
                            and a.data_aula::date = ANY(@datas)
                            and raa.codigo_aluno = ANY(@alunoCodigos)
                            group by raa.codigo_aluno, a.quantidade, a.data_aula) a
                            where a.quantidade = a.faltas";

            return await database.Conexao.QueryAsync<AusenciaAlunoDto>(query, new { turmaCodigo, disciplinaCodigo, datas, alunoCodigos });
        }

        public IEnumerable<RegistroAusenciaAluno> ObterListaFrequenciaPorAula(long aulaId)
        {
            var query = @"select ra.*
                        from
	                        registro_frequencia rf
                        inner join registro_ausencia_aluno ra on
	                        rf.id = ra.registro_frequencia_id
                        inner join aula a on
	                        a.id = rf.aula_id
                        where ra.excluido = false and
	                        a.id = @aulaId";

            return database.Conexao.Query<RegistroAusenciaAluno>(query, new { aulaId });
        }

        public RegistroFrequencia ObterRegistroFrequenciaPorAulaId(long aulaId)
        {
            var query = @"select *
                            from registro_frequencia
                          where not excluido
                            and aula_id = @aulaId";

            return database.Conexao.QueryFirstOrDefault<RegistroFrequencia>(query, new { aulaId });
        }
    }
}