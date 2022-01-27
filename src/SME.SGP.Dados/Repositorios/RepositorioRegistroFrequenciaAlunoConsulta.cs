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
            var query = @"select 
                        rfa.codigo_aluno as CodigoAluno,
                        rfa.numero_aula as NumeroAula,
                        rfa.valor as TipoFrequencia
                        from registro_frequencia rf 
                        inner join registro_frequencia_aluno rfa on rf.id = rfa.registro_frequencia_id 
                        where not rf.excluido
                          and not rfa.excluido
                          and rf.aula_id = @aulaId";

            return await database.Conexao.QueryAsync<FrequenciaAlunoSimplificadoDto>(query, new { aulaId });
        }

        public Task<IEnumerable<RegistroFrequenciaAluno>> ObterRegistrosAusenciaPorAula(long aulaId)
        {
            var query = @"select a.* 
                      from registro_frequencia_aluno a
                      inner join registro_frequencia f on f.id = a.registro_frequencia_id
                      where not f.excluido and not a.excluido
                        and a.valor = @tipoFrequencia
                        and f.aula_id = @aulaId ";

            return database.Conexao.QueryAsync<RegistroFrequenciaAluno>(query, new { aulaId, tipoFrequencia = (int)TipoFrequencia.F });
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
                inner join registro_frequencia rf on
	                rfa.registro_frequencia_id = rf.id
                inner join aula a on
	                rf.aula_id = a.id
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
                            join registro_frequencia rf on rfa.registro_frequencia_id = rf.id
                            join aula a on rf.aula_id = a.id
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
	                count(distinct(rfa.registro_frequencia_id*rfa.numero_aula)) filter (where rfa.valor = 1) as TotalPresencas,
                    count(distinct(rfa.registro_frequencia_id*rfa.numero_aula)) filter (where rfa.valor = 2) as TotalAusencias,
                    count(distinct(rfa.registro_frequencia_id*rfa.numero_aula)) filter (where rfa.valor = 3) as TotalRemotos,
	                p.id as PeriodoEscolarId,
	                p.periodo_inicio as PeriodoInicio,
	                p.periodo_fim as PeriodoFim,
	                p.bimestre,
                    rfa.codigo_aluno as AlunoCodigo,
                    a.disciplina_id as ComponenteCurricularId
                from
	                registro_frequencia_aluno rfa
                inner join registro_frequencia rf on
	                rfa.registro_frequencia_id = rf.id
                inner join aula a on
	                rf.aula_id = a.id
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
            var query = @"select a.* 
                      from registro_frequencia_aluno a
                      inner join registro_frequencia f on f.id = a.registro_frequencia_id
                      where not a.excluido and not f.excluido
                        and a.valor = @tipo
                        and f.aula_id = @aulaId ";

            return database.Conexao.QueryAsync<RegistroFrequenciaAluno>(query, new { aulaId, tipo = (int)TipoFrequencia.F });
        }

        public Task<IEnumerable<FrequenciaAlunoAulaDto>> ObterFrequenciasDoAlunoNaAula(string codigoAluno, long aulaId)
        {
            var query = @"select
                            fa.id as FrequenciaAlunoCodigo,
                            fa.valor TipoFrequencia,
                            fa.numero_aula as NumeroAula,
                            fa.codigo_aluno as AlunoCodigo 
                            from registro_frequencia_aluno fa
                            join registro_frequencia rf on fa.registro_frequencia_id = rf.id
                            join aula a on rf.aula_id = a.id
                            where not fa.excluido and not rf.excluido and not a.excluido
                            and codigo_aluno = @codigoAluno and a.id = @aulaId
                            order by fa.id desc";

            return database.Conexao.QueryAsync<FrequenciaAlunoAulaDto>(query, new { aulaId, codigoAluno });
        }


        public async Task<int> ObterTotalAulasPorDisciplinaETurma(DateTime dataAula, string disciplinaId, params string[] turmasId)
        {
            String query = BuildQueryObterTotalAulasPorDisciplinaETurma(disciplinaId);
            return await database.Conexao.QueryFirstOrDefaultAsync<int>(query.ToString(), new { dataAula, disciplinaId, turmasId });
        }

        private String BuildQueryObterTotalAulasPorDisciplinaETurma(string disciplinaId)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select ");
            query.AppendLine("COALESCE(SUM(a.quantidade),0) AS total");
            query.AppendLine("from ");
            query.AppendLine("aula a ");
            query.AppendLine("inner join registro_frequencia rf on ");
            query.AppendLine("rf.aula_id = a.id ");
            query.AppendLine("inner join periodo_escolar p on ");
            query.AppendLine("a.tipo_calendario_id = p.tipo_calendario_id ");
            query.AppendLine("where not a.excluido");
            query.AppendLine("and p.periodo_inicio <= @dataAula ");
            query.AppendLine("and p.periodo_fim >= @dataAula ");
            query.AppendLine("and a.data_aula >= p.periodo_inicio");
            query.AppendLine("and a.data_aula <= p.periodo_fim ");

            if (!string.IsNullOrWhiteSpace(disciplinaId))
                query.AppendLine("and a.disciplina_id = @disciplinaId ");

            query.AppendLine("and a.turma_id = any(@turmasId) ");
            return query.ToString();
        }
    }
}
