using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRegistroAusenciaAluno : RepositorioBase<RegistroAusenciaAluno>, IRepositorioRegistroAusenciaAluno
    {
        private readonly string connectionString;
        public RepositorioRegistroAusenciaAluno(ISgpContext database, IConfiguration configuration) : base(database)
        {
            this.connectionString = configuration.GetConnectionString("SGP_Postgres");
        }

        public bool MarcarRegistrosAusenciaAlunoComoExcluidoPorRegistroFrequenciaId(long registroFrequenciaId)
        {
            var query = @"update
                            registro_ausencia_aluno
                        set
                            excluido = true
                        where
                            registro_frequencia_id = @registroFrequenciaId";

            return database.Conexao.Execute(query, new { registroFrequenciaId }) > 0;
        }

        public Task<IEnumerable<RegistroAusenciaAluno>> ObterRegistrosAusenciaPorAulaAsync(long aulaId)
        {
            var query = @"select a.* 
                      from registro_ausencia_aluno a
                      inner join registro_frequencia f on f.id = a.registro_frequencia_id
                      where f.aula_id = @aulaId ";

            return database.Conexao.QueryAsync<RegistroAusenciaAluno>(query, new { aulaId });
        }

        private String BuildQueryObterTotalAusenciasPorAlunoETurma(DateTime dataAula, string codigoAluno,
            string disciplinaId, string turmaId)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select");
            query.AppendLine("count(ra.id) as TotalAusencias, ");
            query.AppendLine("p.id as PeriodoEscolarId, ");
            query.AppendLine("p.periodo_inicio as PeriodoInicio, ");
            query.AppendLine("p.periodo_fim as PeriodoFim, ");
            query.AppendLine("p.bimestre ");
            query.AppendLine("from ");
            query.AppendLine("registro_ausencia_aluno ra ");
            query.AppendLine("inner join registro_frequencia rf on ");
            query.AppendLine("ra.registro_frequencia_id = rf.id ");
            query.AppendLine("inner join aula a on ");
            query.AppendLine("rf.aula_id = a.id ");
            query.AppendLine("inner join periodo_escolar p on ");
            query.AppendLine("a.tipo_calendario_id = p.tipo_calendario_id ");
            query.AppendLine("where not ra.excluido and not a.excluido and ");
            query.AppendLine("ra.codigo_aluno = @codigoAluno ");

            if (!string.IsNullOrWhiteSpace(disciplinaId))
                query.AppendLine("and a.disciplina_id = @disciplinaId ");

            //TODO: filtro da turma?
            query.AppendLine("and a.turma_id = @turmaId ");

            query.AppendLine("and p.periodo_inicio <= @dataAula ");
            query.AppendLine("and p.periodo_fim >= @dataAula ");
            query.AppendLine("and a.data_aula >= p.periodo_inicio");
            query.AppendLine("and a.data_aula <= p.periodo_fim ");
            query.AppendLine("and not ra.excluido");
            query.AppendLine("and not a.excluido");
            query.AppendLine("group by");
            query.AppendLine("p.id, ");
            query.AppendLine("p.periodo_inicio,");
            query.AppendLine("p.periodo_fim,");
            query.AppendLine("p.bimestre");
            return query.ToString();
        }

        public AusenciaPorDisciplinaDto ObterTotalAusenciasPorAlunoETurma(DateTime dataAula, string codigoAluno, string disciplinaId, string turmaId)
        {
            String query = BuildQueryObterTotalAusenciasPorAlunoETurma(dataAula, codigoAluno, disciplinaId, turmaId);
            return database.Conexao.QueryFirstOrDefault<AusenciaPorDisciplinaDto>(query.ToString(), new { dataAula, codigoAluno, disciplinaId, turmaId });
        }

        public async Task<AusenciaPorDisciplinaDto> ObterTotalAusenciasPorAlunoETurmaAsync(DateTime dataAula, string codigoAluno, string disciplinaId, string turmaId)
        {
            String query = BuildQueryObterTotalAusenciasPorAlunoETurma(dataAula, codigoAluno, disciplinaId, turmaId);
            return await database.Conexao.QueryFirstOrDefaultAsync<AusenciaPorDisciplinaDto>(query.ToString(), new { dataAula, codigoAluno, disciplinaId, turmaId });
        }

        public async Task<IEnumerable<AusenciaPorDisciplinaAlunoDto>> ObterTotalAusenciasPorAlunosETurmaAsync(DateTime dataAula, IEnumerable<string> codigoAlunos, string turmaId)
        {
            var query = @"           
                    select
	                count(ra.id) as TotalAusencias,
	                p.id as PeriodoEscolarId,
	                p.periodo_inicio as PeriodoInicio,
	                p.periodo_fim as PeriodoFim,
	                p.bimestre,
                    ra.codigo_aluno as AlunoCodigo,
                    a.disciplina_id as ComponenteCurricularId                    
                from
	                registro_ausencia_aluno ra
                inner join registro_frequencia rf on
	                ra.registro_frequencia_id = rf.id
                inner join aula a on
	                rf.aula_id = a.id
                inner join periodo_escolar p on
	                a.tipo_calendario_id = p.tipo_calendario_id
                where
	                not ra.excluido
	                and not a.excluido
	                and ra.codigo_aluno = any(@codigoAlunos)	                
	                and a.turma_id = @turmaId
	                and p.periodo_inicio <= @dataAula
	                and p.periodo_fim >= @dataAula
	                and a.data_aula >= p.periodo_inicio
	                and a.data_aula <= p.periodo_fim
	                and not ra.excluido
	                and not a.excluido
                group by
	                p.id,
	                p.periodo_inicio,
	                p.periodo_fim,
	                p.bimestre,
                    ra.codigo_aluno,
                    a.disciplina_id";

            return await database.Conexao.QueryAsync<AusenciaPorDisciplinaAlunoDto>(query, new { dataAula, codigoAlunos, turmaId });
        }

        public async Task SalvarVarios(List<RegistroAusenciaAluno> entidades)
        {
            var sql = @"copy registro_ausencia_aluno (                                         
                                        codigo_aluno, 
                                        numero_aula, 
                                        registro_frequencia_id,
                                        migrado,
                                        criado_por,
                                        criado_rf,
                                        criado_em)
                            from
                            stdin (FORMAT binary)";

            using (var conexao = new NpgsqlConnection(connectionString))
            {
                await conexao.OpenAsync();
                using (var writer = conexao.BeginBinaryImport(sql))
                {
                    foreach (var entidade in entidades)
                    {
                        writer.StartRow();
                        writer.Write(entidade.CodigoAluno, NpgsqlDbType.Varchar);
                        writer.Write(entidade.NumeroAula, NpgsqlDbType.Integer); ;
                        writer.Write(entidade.RegistroFrequenciaId, NpgsqlDbType.Bigint);
                        writer.Write(entidade.Migrado, NpgsqlDbType.Boolean);
                        writer.Write(database.UsuarioLogadoNomeCompleto, NpgsqlDbType.Varchar);
                        writer.Write(database.UsuarioLogadoRF, NpgsqlDbType.Varchar);
                        writer.Write(entidade.CriadoEm, NpgsqlDbType.Timestamp);
                    }
                    await Task.FromResult(writer.Complete());
                    conexao.Close();
                }
            }
        }

        public async Task ExcluirVarios(List<long> idsParaExcluir)
        {
            var query = "delete from registro_ausencia_aluno where = any(@idsParaExcluir)";

            using (var conexao = new NpgsqlConnection(connectionString))
            {
                await conexao.OpenAsync();
                await conexao.ExecuteAsync(
                    query,
                    new
                    {
                        idsParaExcluir

                    });
                conexao.Close();
            }
        }
    }
}