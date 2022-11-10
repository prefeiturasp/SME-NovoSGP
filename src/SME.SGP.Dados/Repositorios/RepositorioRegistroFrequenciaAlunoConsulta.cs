using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRegistroFrequenciaAlunoConsulta : IRepositorioRegistroFrequenciaAlunoConsulta
    {
        private readonly ISgpContextConsultas database;

        public RepositorioRegistroFrequenciaAlunoConsulta(ISgpContextConsultas database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<IEnumerable<FrequenciaAlunoSimplificadoDto>> ObterFrequenciasPorAulaId(long aulaId)
        {
            var query = @"SELECT rfa.codigo_aluno AS CodigoAluno,
                               rfa.numero_aula AS NumeroAula,
                               rfa.valor AS TipoFrequencia
                        FROM registro_frequencia_aluno rfa
                        WHERE NOT rfa.excluido
                          AND rfa.aula_id = @aulaId";

            return await database.Conexao.QueryAsync<FrequenciaAlunoSimplificadoDto>(query, new { aulaId });
        }

        public Task<IEnumerable<RegistroFrequenciaAluno>> ObterRegistrosAusenciaPorAula(long aulaId)
        {
            var query = @"SELECT *
                            FROM registro_frequencia_aluno rfa
                            WHERE NOT rfa.excluido
                              AND rfa.valor = @tipoFrequencia
                              AND rfa.aula_id = @aulaId";

            return database.Conexao.QueryAsync<RegistroFrequenciaAluno>(query, new { aulaId, tipoFrequencia = (int)TipoFrequencia.F });
        }

        public Task<IEnumerable<RegistroFrequenciaAluno>> ObterRegistrosFrequenciaPorAulaId(long aulaId)
        {
            var query = @"SELECT *
                            FROM registro_frequencia_aluno rfa
                            WHERE NOT rfa.excluido
                              AND rfa.aula_id = @aulaId";

            return database.Conexao.QueryAsync<RegistroFrequenciaAluno>(query, new { aulaId });
        }

        public async Task<IEnumerable<AusenciaPorDisciplinaAlunoDto>> ObterAusenciasAlunosPorAlunosETurmaIdEDataAula(DateTime dataAula, IEnumerable<string> codigoAlunos, params string[] turmasId)
        {
            var query = @"           
                    select
	                count(distinct(rfa.registro_frequencia_id*rfa.numero_aula)) as TotalAusencias,
	                p.id as PeriodoEscolarId,
	                p.periodo_inicio as PeriodoInicio,
	                p.periodo_fim as PeriodoFim,
	                p.bimestre,
                    rfa.codigo_aluno as AlunoCodigo,
                    a.disciplina_id as ComponenteCurricularId                    
                from
                    registro_frequencia_aluno rfa
                inner join aula a on
                    rfa.aula_id = a.id
                inner join periodo_escolar p on
                    a.tipo_calendario_id = p.tipo_calendario_id
                where
	                not rfa.excluido
	                and not a.excluido
	                and rfa.codigo_aluno = any(@codigoAlunos)	                
	                and a.turma_id = any(@turmasId)
	                and p.periodo_inicio <= @dataAula
	                and p.periodo_fim >= @dataAula
	                and a.data_aula >= p.periodo_inicio
	                and a.data_aula <= p.periodo_fim
                    and rfa.valor = @tipoFrequencia
                    and rfa.numero_aula <= a.quantidade 
                group by
	                p.id,
	                p.periodo_inicio,
	                p.periodo_fim,
	                p.bimestre,
                    rfa.codigo_aluno,
                    a.disciplina_id";

            return await database.Conexao.QueryAsync<AusenciaPorDisciplinaAlunoDto>(query, new { dataAula, codigoAlunos, turmasId, tipoFrequencia = (int)TipoFrequencia.F });
        }
        public async Task<IEnumerable<RegistroFrequenciaGeralPorDisciplinaAlunoTurmaDataDto>> ObterFrequenciaAlunosGeralPorAnoQuery(int ano)
        {
            var query = @"           
                        select distinct   
                            a.disciplina_id as DisciplinaId,
                            a.turma_id as TurmaId,
                            p.periodo_fim as DataAula,
                            rfa.codigo_aluno as AlunoCodigo
                        from
                            registro_frequencia_aluno rfa
                            join aula a on rfa.aula_id = a.id
                            join periodo_escolar p on a.tipo_calendario_id = p.tipo_calendario_id
                        where
                            not rfa.excluido
                            and not a.excluido
                            and extract(year from p.periodo_inicio) = @ano    
                            and a.data_aula >= p.periodo_inicio
                            and a.data_aula <= p.periodo_fim                    
                            and rfa.numero_aula <= a.quantidade                            
                        order by a.disciplina_id,
		                         a.turma_id,
		                         p.periodo_fim,
		                         rfa.codigo_aluno";

            return await database.Conexao.QueryAsync<RegistroFrequenciaGeralPorDisciplinaAlunoTurmaDataDto>(query, new { ano });
        }

        public async Task<IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto>> ObterRegistroFrequenciaAlunosPorAlunosETurmaIdEDataAula(DateTime dataAula, string[] turmasId, IEnumerable<string> codigoAlunos)
        {
            var query = @"           
                    select
	                count(distinct(rfa.aula_id*rfa.numero_aula)) filter (where rfa.valor = 1) as TotalPresencas,
                    count(distinct(rfa.aula_id*rfa.numero_aula)) filter (where rfa.valor = 2) as TotalAusencias,
                    count(distinct(rfa.aula_id*rfa.numero_aula)) filter (where rfa.valor = 3) as TotalRemotos,
	                p.id as PeriodoEscolarId,
	                p.periodo_inicio as PeriodoInicio,
	                p.periodo_fim as PeriodoFim,
	                p.bimestre,
                    rfa.codigo_aluno as AlunoCodigo,
                    a.disciplina_id as ComponenteCurricularId
                from
	                registro_frequencia_aluno rfa
                inner join aula a on
	                rfa.aula_id = a.id
                inner join periodo_escolar p on
	                a.tipo_calendario_id = p.tipo_calendario_id
                where
	                not rfa.excluido
	                and not a.excluido
	                and rfa.codigo_aluno = any(@codigoAlunos)	                
	                and a.turma_id = any(@turmasId)
	                and p.periodo_inicio <= @dataAula
	                and p.periodo_fim >= @dataAula
	                and a.data_aula >= p.periodo_inicio
	                and a.data_aula <= p.periodo_fim                    
                    and rfa.numero_aula <= a.quantidade 
                group by
	                p.id,
	                p.periodo_inicio,
	                p.periodo_fim,
	                p.bimestre,
                    rfa.codigo_aluno,
                    a.disciplina_id";

            return await database.Conexao.QueryAsync<RegistroFrequenciaPorDisciplinaAlunoDto>(query, new { dataAula, codigoAlunos, turmasId });
        }

        public Task<IEnumerable<RegistroFrequenciaAluno>> ObterRegistrosAusenciaPorAulaAsync(long aulaId)
        {
            var query = @"select * 
                          from registro_frequencia_aluno rfa
                          where not rfa.excluido and rfa.valor = @tipo
                            and rfa.aula_id = @aulaId ";

            return database.Conexao.QueryAsync<RegistroFrequenciaAluno>(query, new { aulaId, tipo = (int)TipoFrequencia.F });
        }

        public Task<IEnumerable<FrequenciaAlunoAulaDto>> ObterFrequenciasDoAlunoNaAula(string codigoAluno, long aulaId)
        {
            var query = @"select
	                        rfa.id as FrequenciaAlunoCodigo,
	                        rfa.valor TipoFrequencia,
	                        rfa.numero_aula as NumeroAula,
	                        rfa.codigo_aluno as AlunoCodigo 
                        from registro_frequencia_aluno rfa
	                        join aula a on rfa.aula_id = a.id
                        where not rfa.excluido and not a.excluido
	                        and rfa.codigo_aluno = @codigoAluno and a.id = @aulaId
	                        order by rfa.id desc";

            return database.Conexao.QueryAsync<FrequenciaAlunoAulaDto>(query, new { aulaId, codigoAluno });
        }

        public async Task<int> ObterTotalAulasPorDisciplinaETurma(DateTime dataAula, string disciplinaId, DateTime? dataMatriculaAluno = null, DateTime? dataSituacaoAluno = null, params string[] turmasId)
        {
            var query = BuildQueryObterTotalAulasPorDisciplinaETurma(disciplinaId, dataMatriculaAluno, dataSituacaoAluno);
            return await database.Conexao.QueryFirstOrDefaultAsync<int>(query.ToString(),
                new { dataAula, disciplinaId, turmasId, dataMatriculaAluno = dataMatriculaAluno.HasValue ? dataMatriculaAluno.Value.Date : (DateTime?)null, dataSituacaoAluno = dataSituacaoAluno.HasValue ? dataSituacaoAluno.Value.Date.AddDays(-1) : (DateTime?)null });
        }

        private string BuildQueryObterTotalAulasPorDisciplinaETurma(string disciplinaId, DateTime? dataMatriculaAluno = null, DateTime? dataSituacaoAluno = null)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select COALESCE(SUM(a.quantidade),0) AS total");
            query.AppendLine("  from aula a ");
            query.AppendLine("      inner join periodo_escolar p on");
            query.AppendLine("          a.tipo_calendario_id = p.tipo_calendario_id");
            query.AppendLine("where not a.excluido");
            query.AppendLine("and @dataAula::date between p.periodo_inicio and p.periodo_fim");
            query.AppendLine("and a.data_aula::date between p.periodo_inicio and p.periodo_fim");

            if (!string.IsNullOrWhiteSpace(disciplinaId))
                query.AppendLine("and a.disciplina_id = @disciplinaId");

            if (dataMatriculaAluno.HasValue && dataSituacaoAluno.HasValue)
                query.AppendLine("and a.data_aula::date between @dataMatriculaAluno::date and @dataSituacaoAluno::date - 1");
            else if (dataMatriculaAluno.HasValue)
                query.AppendLine("and a.data_aula::date >= @dataMatriculaAluno::date");
            else if (dataSituacaoAluno.HasValue)
                query.AppendLine("and a.data_aula::date < @dataSituacaoAluno::date");

            query.AppendLine("and a.turma_id = any(@turmasId);");           

            return query.ToString();
        }

        public async Task<IEnumerable<RegistroFrequenciaAlunoPorTurmaEMesDto>> ObterRegistroFrequenciaAlunosPorTurmaEMes(string turmaCodigo, int mes)
        {
            const string query = @"SELECT t.id AS TurmaId,
									       rfa.codigo_aluno AS AlunoCodigo,
									       sum(a.quantidade) AS QuantidadeAulas,
									       count(distinct(rfa.registro_frequencia_id * rfa.numero_aula)) filter (
									        WHERE rfa.valor = 2) AS QuantidadeAusencias,
									       sum(0) AS QuantidadeCompensacoes,
									       t.ano_letivo
									FROM registro_frequencia_aluno rfa
									INNER JOIN aula a ON a.id = rfa.aula_id
									AND NOT a.excluido
									INNER JOIN turma t ON t.turma_id = a.turma_id
									WHERE NOT rfa.excluido
									  AND a.turma_id = @turmaCodigo
									  AND extract(MONTH
									              FROM a.data_aula) = @mes
									GROUP BY rfa.codigo_aluno,
									         t.id,
									         t.ano_letivo
									ORDER BY t.ano_letivo,
									         rfa.codigo_aluno";

            var parametros = new { turmaCodigo, mes };

            return await database.Conexao.QueryAsync<RegistroFrequenciaAlunoPorTurmaEMesDto>(query, parametros);
        }

        public async Task<IEnumerable<FrequenciaAlunoTurmaDto>> ObterRegistroFrequenciaAlunosNaTurma(string turmaCodigo, string alunoCodigo)
        {
            const string query = @"select  a.id as AulaId, rfa.id as RegistroFrequenciaAlunoId, a.data_aula as DataAula, a.disciplina_id as DisciplinaCodigo, rfa.valor as Valor
                                            from registro_frequencia_aluno rfa 
                                            inner join registro_frequencia rf on rf.id  = rfa.registro_frequencia_id 
                                            inner join aula a on a.id = rf.aula_id 
                                            where a.turma_id = @turmaCodigo and rfa.codigo_aluno = @alunoCodigo
                                            and not rfa.excluido and not a.excluido 
                                            order by a.data_aula";

            var parametros = new { turmaCodigo, alunoCodigo };

            return await database.Conexao.QueryAsync<FrequenciaAlunoTurmaDto>(query, parametros);
        }

        public Task<IEnumerable<RegistroFrequenciaAluno>> ObterRegistrosAusenciaPorIdRegistro(long registroFrequenciaId)
        {
            var query = @"SELECT
							id,
							valor,
							codigo_aluno,
							numero_aula,
							registro_frequencia_id,
							criado_em,
							criado_por,
							criado_rf,
							alterado_em,
							alterado_por,
							alterado_rf,
							excluido,
							migrado,
							aula_id
						FROM
							registro_frequencia_aluno
                        where not excluido 
                            and registro_frequencia_id = @registroFrequenciaId";
            return database.Conexao.QueryAsync<RegistroFrequenciaAluno>(query, new { registroFrequenciaId });
        }
    }
}
