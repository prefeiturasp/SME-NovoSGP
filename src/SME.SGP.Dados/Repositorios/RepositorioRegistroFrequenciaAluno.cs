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

        public async Task<IEnumerable<AusenciaPorDisciplinaAlunoDto>> ObterAusenciasAlunosPorAlunosETurmaIdEDataAula(DateTime dataAula, string turmaId, IEnumerable<string> codigoAlunos)
        {
            var query = @"           
                    select
	                count(rfa.id) as TotalAusencias,
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
	                and a.turma_id = @turmaId
	                and p.periodo_inicio <= @dataAula
	                and p.periodo_fim >= @dataAula
	                and a.data_aula >= p.periodo_inicio
	                and a.data_aula <= p.periodo_fim
	                and not a.excluido 
                    and rfa.valor = @tipoFrequencia
                group by
	                p.id,
	                p.periodo_inicio,
	                p.periodo_fim,
	                p.bimestre,
                    rfa.codigo_aluno,
                    a.disciplina_id";

            return await database.Conexao.QueryAsync<AusenciaPorDisciplinaAlunoDto>(query, new { dataAula, codigoAlunos, turmaId, tipoFrequencia = (int)TipoFrequencia.F });
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
                    writer.Write(frequencia.Valor, NpgsqlDbType.Integer);
                    writer.Write(frequencia.CodigoAluno, NpgsqlDbType.Varchar);
                    writer.Write(frequencia.NumeroAula, NpgsqlDbType.Integer);
                    writer.Write(frequencia.RegistroFrequenciaId, NpgsqlDbType.Bigint);
                    writer.Write(frequencia.CriadoEm, NpgsqlDbType.Timestamp);
                    writer.Write(frequencia.CriadoPor, NpgsqlDbType.Varchar);
                    writer.Write(frequencia.CriadoRF, NpgsqlDbType.Varchar);
                }
                await Task.FromResult(writer.Complete());
            }

            return true;
        }
    }
}
