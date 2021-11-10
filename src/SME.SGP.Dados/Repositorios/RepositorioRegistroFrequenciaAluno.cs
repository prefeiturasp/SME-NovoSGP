using Dapper;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioRegistroFrequenciaAluno : RepositorioBase<RegistroFrequenciaAluno>, IRepositorioRegistroFrequenciaAluno
    {
        public RepositorioRegistroFrequenciaAluno(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<RegistroFrequenciaGeralPorDisciplinaAlunoTurmaDataDto>> ObterFrequenciaAlunosGeralPorAnoQuery(int ano)
        {
            var query = @"           
                        select distinct   
                            a.disciplina_id as DisciplinaId,
                            a.turma_id as TurmaId,
                            a.data_aula as DataAula,
                            rfa.codigo_aluno as AlunoCodigo
                        from
                            registro_frequencia_aluno rfa
                        inner join registro_frequencia rf on rfa.registro_frequencia_id = rf.id
                        inner join aula a on rf.aula_id = a.id
                        inner join periodo_escolar p on a.tipo_calendario_id = p.tipo_calendario_id
                        where
                            not rfa.excluido
                            and not a.excluido
                            and extract(year from p.periodo_inicio) = @ano    
                            and a.data_aula >= p.periodo_inicio
                            and a.data_aula <= p.periodo_fim                    
                            and rfa.numero_aula <= a.quantidade
                        order by a.disciplina_id,
		                         a.turma_id,
		                         a.data_aula,
		                         rfa.codigo_aluno";

            return await database.Conexao.QueryAsync<RegistroFrequenciaGeralPorDisciplinaAlunoTurmaDataDto>(query, new { ano});
        }

        public async Task<IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto>> ObterRegistroFrequenciaAlunosPorAlunosETurmaIdEDataAula(DateTime dataAula, string turmaId, IEnumerable<string> codigoAlunos)
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
                    a.disciplina_id as ComponenteCurricularId,
                    rfa.valor as TipoFrequencia
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
	                and a.turma_id = @turmaId
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
                    a.disciplina_id,
                    rfa.valor";

            return await database.Conexao.QueryAsync<RegistroFrequenciaPorDisciplinaAlunoDto>(query, new { dataAula, codigoAlunos, turmaId });
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

        public async Task RemoverPorRegistroFrequenciaId(long registroFrequenciaId)
        {
            await database.Conexao.ExecuteAsync("DELETE FROM registro_frequencia_aluno WHERE registro_frequencia_id = @registroFrequenciaId", 
                new { registroFrequenciaId });
        }

        public async Task<bool> InserirVarios(IEnumerable<RegistroFrequenciaAluno> registros)
        {
            var sql = @"copy registro_frequencia_aluno (                                         
                                        valor, 
                                        codigo_aluno, 
                                        numero_aula, 
                                        registro_frequencia_id, 
                                        criado_em,
                                        criado_por,                                        
                                        criado_rf)
                            from
                            stdin (FORMAT binary)";

            using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
            {
                foreach (var frequencia in registros)
                {
                    writer.StartRow();
                    writer.Write(frequencia.Valor, NpgsqlDbType.Bigint);
                    writer.Write(frequencia.CodigoAluno);
                    writer.Write(frequencia.NumeroAula);
                    writer.Write(frequencia.RegistroFrequenciaId);
                    writer.Write(frequencia.CriadoEm);
                    writer.Write(frequencia.CriadoPor);
                    writer.Write(frequencia.CriadoRF);
                }
                writer.Complete();
            }

            return true;
        }

        public async Task ExcluirVarios(List<long> idsParaExcluir)
        {
            var query = "delete from registro_frequencia_aluno where = any(@idsParaExcluir)";

            using (var conexao = (NpgsqlConnection)database.Conexao)
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
