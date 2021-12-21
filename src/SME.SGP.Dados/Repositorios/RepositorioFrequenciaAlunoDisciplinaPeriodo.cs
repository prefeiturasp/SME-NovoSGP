using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequenciaAlunoDisciplinaPeriodo : RepositorioBase<FrequenciaAluno>, IRepositorioFrequenciaAlunoDisciplinaPeriodo
    {
        private readonly string connectionString;

        public RepositorioFrequenciaAlunoDisciplinaPeriodo(ISgpContext database, IConfiguration configuration) : base(database)
        {
            this.connectionString = configuration.GetConnectionString("SGP_Postgres");
        }

        public async Task SalvarVariosAsync(IEnumerable<FrequenciaAluno> entidades)
        {
            var sql = @"copy frequencia_aluno (                                         
                                        codigo_aluno, 
                                        tipo, 
                                        disciplina_id, 
                                        periodo_inicio, 
                                        periodo_fim, 
                                        bimestre, 
                                        total_aulas, 
                                        total_ausencias, 
                                        criado_em,
                                        criado_por,                                        
                                        criado_rf,
                                        total_compensacoes,
                                        turma_id,
                                        periodo_escolar_id)
                            from
                            stdin (FORMAT binary)";

            using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
            {
                foreach (var frequencia in entidades)
                {
                    writer.StartRow();
                    writer.Write(frequencia.CodigoAluno, NpgsqlDbType.Varchar);
                    writer.Write((int)frequencia.Tipo, NpgsqlDbType.Integer);
                    writer.Write(frequencia.DisciplinaId, NpgsqlDbType.Varchar);
                    writer.Write(frequencia.PeriodoInicio, NpgsqlDbType.Timestamp);
                    writer.Write(frequencia.PeriodoFim, NpgsqlDbType.Timestamp);
                    writer.Write(frequencia.Bimestre, NpgsqlDbType.Integer);
                    writer.Write(frequencia.TotalAulas, NpgsqlDbType.Integer);
                    writer.Write(frequencia.TotalAusencias, NpgsqlDbType.Integer);
                    writer.Write(frequencia.CriadoEm, NpgsqlDbType.Timestamp);
                    writer.Write(database.UsuarioLogadoNomeCompleto, NpgsqlDbType.Varchar);
                    writer.Write(database.UsuarioLogadoRF, NpgsqlDbType.Varchar);
                    writer.Write(frequencia.TotalCompensacoes, NpgsqlDbType.Integer);
                    writer.Write(frequencia.TurmaId, NpgsqlDbType.Varchar);

                    if (frequencia.PeriodoEscolarId.HasValue)
                        writer.Write((long)frequencia.PeriodoEscolarId, NpgsqlDbType.Bigint);
                }
                await Task.FromResult(writer.Complete());
            }
        }
        public async Task RemoverVariosAsync(long[] ids)
        {
            var query = @"delete from frequencia_aluno where id = any(@ids)";

            using (var conexao = new NpgsqlConnection(connectionString))
            {
                await conexao.OpenAsync();
                var transacao = conexao.BeginTransaction();
                try
                {
                    await conexao.ExecuteAsync(query, new
                    {
                        ids
                    }, transacao);
                    await transacao.CommitAsync();
                    conexao.Close();
                }
                catch (Exception)
                {
                    await transacao.RollbackAsync();
                    throw;
                }
            }
        }
        public async Task RemoverFrequenciaGeralAlunos(string[] alunos, string turmaCodigo, long periodoEscolarId)
        {
            var query = @"delete from frequencia_aluno 
                        where tipo = 2 
                          and turma_id = @turmaCodigo 
                          and codigo_aluno = any(@alunos) 
                          and periodo_escolar_id = @periodoEscolarId";

            await database.Conexao.ExecuteAsync(query, new { alunos, turmaCodigo, periodoEscolarId });
        }
        public async Task RemoverFrequenciasDuplicadas(string[] alunos, string turmaCodigo, long periodoEscolarId)
        {
            var query = @"select fa.turma_id as TurmaCodigo
                               , fa.codigo_aluno as AlunoCodigo
                               , fa.disciplina_id as DisciplinaId
                               , fa.periodo_escolar_id as PeriodoEscolarId
                               , max(id) as UltimoId
                      from frequencia_aluno fa 
                    where fa.turma_id = @turmaCodigo
                      and fa.codigo_aluno = any(@alunos)
                      and fa.periodo_escolar_id = @periodoEscolarId
                    group by fa.turma_id, fa.codigo_aluno, fa.disciplina_id, fa.periodo_escolar_id  
                    having count(id) > 1 ";

            var duplicados = await database.Conexao.QueryAsync<RegistroFrequenciaDuplicadoDto>(query, new { alunos, turmaCodigo, periodoEscolarId });

            if (duplicados != null && duplicados.Any())
            {
                var delete = @"delete
                                from frequencia_aluno fa 
                            where fa.turma_id = @turmaCodigo
                                and fa.codigo_aluno = @alunoCodigo
                                and fa.periodo_escolar_id = @periodoEscolarId
                                and fa.disciplina_id = @disciplinaId
                                and fa.id <> @ultimoId";

                foreach (var duplicado in duplicados)
                {
                    await database.Conexao.ExecuteAsync(delete, new { duplicado.TurmaCodigo, duplicado.AlunoCodigo, duplicado.PeriodoEscolarId, duplicado.DisciplinaId, duplicado.UltimoId });
                }
            }

        }
    }
}