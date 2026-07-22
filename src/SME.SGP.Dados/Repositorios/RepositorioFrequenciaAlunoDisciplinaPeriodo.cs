using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequenciaAlunoDisciplinaPeriodo : RepositorioBase<FrequenciaAluno>, IRepositorioFrequenciaAlunoDisciplinaPeriodo
    {
        public RepositorioFrequenciaAlunoDisciplinaPeriodo(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task SalvarVariosAsync(IEnumerable<FrequenciaAluno> entidades)
        {
            const string sql = @"copy frequencia_aluno (                                         
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
                                        periodo_escolar_id,
                                        professor_rf)
                            from
                            stdin (FORMAT binary)";

            await using var writer = await ((NpgsqlConnection)database.Conexao).BeginBinaryImportAsync(sql);
            foreach (var frequencia in entidades)
            {
                await writer.StartRowAsync();
                await writer.WriteAsync(frequencia.CodigoAluno, NpgsqlDbType.Varchar); 
                await writer.WriteAsync((int)frequencia.Tipo, NpgsqlDbType.Integer);
                await writer.WriteAsync(frequencia.DisciplinaId, NpgsqlDbType.Varchar);
                await writer.WriteAsync(frequencia.PeriodoInicio, NpgsqlDbType.Timestamp);
                await writer.WriteAsync(frequencia.PeriodoFim, NpgsqlDbType.Timestamp);
                await writer.WriteAsync(frequencia.Bimestre, NpgsqlDbType.Integer);
                await writer.WriteAsync(frequencia.TotalAulas, NpgsqlDbType.Integer);
                await writer.WriteAsync(frequencia.TotalAusencias, NpgsqlDbType.Integer);
                await writer.WriteAsync(frequencia.CriadoEm, NpgsqlDbType.Timestamp);
                await writer.WriteAsync(database.UsuarioLogadoNomeCompleto, NpgsqlDbType.Varchar);
                await writer.WriteAsync(database.UsuarioLogadoRF, NpgsqlDbType.Varchar);
                await writer.WriteAsync(frequencia.TotalCompensacoes, NpgsqlDbType.Integer);
                await writer.WriteAsync(frequencia.TurmaId, NpgsqlDbType.Varchar);
                await writer.WriteAsync(frequencia.Professor, NpgsqlDbType.Varchar);

                if (frequencia.PeriodoEscolarId.HasValue)
                    await writer.WriteAsync((long)frequencia.PeriodoEscolarId, NpgsqlDbType.Bigint);
                else
                    await writer.WriteNullAsync(); 
            }
            await writer.CompleteAsync();
        }
        public async Task RemoverVariosAsync(long[] ids)
        {
            const string comando = @"delete from frequencia_aluno where id in (#ids)";

            for (int i = 0; i < ids.Length; i = i + 900)
            {
                var iteracao = ids.Skip(i).Take(900);

                await database.Conexao.ExecuteAsync(comando.Replace("#ids", string.Join(",", iteracao.Concat(new long[] { 0 }))));
            }
        }
        public async Task RemoverFrequenciaGeralAlunos(string[] alunos, string turmaCodigo, long periodoEscolarId)
        {
            const string query = @"delete from frequencia_aluno 
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

            if (duplicados.NaoEhNulo() && duplicados.Any())
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