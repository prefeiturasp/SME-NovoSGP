using Dapper;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioRegistroFrequenciaAluno : RepositorioBase<RegistroFrequenciaAluno>, IRepositorioRegistroFrequenciaAluno
    {
        public RepositorioRegistroFrequenciaAluno(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public async Task RemoverPorRegistroFrequenciaId(long registroFrequenciaId, string[] alunosComFrequenciaRegistrada)
        {
            await database.Conexao.ExecuteAsync("DELETE FROM registro_frequencia_aluno WHERE registro_frequencia_id = @registroFrequenciaId  and codigo_aluno = any(@alunosComFrequenciaRegistrada);",
            new { registroFrequenciaId, alunosComFrequenciaRegistrada });
        }

        public async Task RemoverPorRegistroFrequenciaIdENumeroAula(long registroFrequenciaId, int numeroAula, string codigoAluno)
        {
            await database.Conexao.ExecuteAsync("DELETE FROM registro_frequencia_aluno WHERE registro_frequencia_id = @registroFrequenciaId AND numero_aula = @numeroAula AND codigo_aluno = @codigoAluno",
                new { registroFrequenciaId, numeroAula, codigoAluno });
        }

        public async Task<bool> InserirVarios(IEnumerable<RegistroFrequenciaAluno> registros)
        {
            return await InserirVariosComLog(registros, false);
        }

        public async Task ExcluirVarios(List<long> idsParaExcluir)
        {
            var query = "delete from registro_frequencia_aluno where id = any(@idsParaExcluir)";

            await database.Conexao.ExecuteAsync(query, new { idsParaExcluir });
        }

        public async Task<bool> InserirVariosComLog(IEnumerable<RegistroFrequenciaAluno> registros)
        {
            return await InserirVariosComLog(registros, true);
        }

        public async Task AlterarRegistroAdicionandoAula(long registroFrequenciaId, long aulaId)
        {
            var query = " update registro_frequencia_aluno set aula_id = @aulaId where registro_frequencia_id = @registroFrequenciaId ";

            await database.Conexao.ExecuteAsync(query, new { aulaId, registroFrequenciaId });
        }

        private async Task<bool> InserirVariosComLog(IEnumerable<RegistroFrequenciaAluno> registros, bool log)
        {
            var sql = @"copy registro_frequencia_aluno (                                         
                                        valor, 
                                        codigo_aluno, 
                                        numero_aula, 
                                        registro_frequencia_id, 
                                        criado_em,
                                        criado_por,                                        
                                        criado_rf,
                                        aula_id)
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
                    writer.Write(log ? database.UsuarioLogadoNomeCompleto : frequencia.CriadoPor);
                    writer.Write(log ? database.UsuarioLogadoRF : frequencia.CriadoRF);
                    writer.Write(frequencia.AulaId);
                }
                writer.Complete();
            }

            return true;
        }

        public async Task ExcluirVariosLogicamente(long[] idsParaExcluir)
        {
            var query = "update registro_frequencia_aluno set excluido = true where id = any(@idsParaExcluir)";

            await database.Conexao.ExecuteAsync(query, new { idsParaExcluir });
        }

        public async Task<IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto>> ObterRegistroFrequenciaAlunosPorAlunosETurmaIdEDataAula(DateTime dataAula, string[] turmasId, IEnumerable<string> codigoAlunos, bool somenteAusencias = false)
        {
            var query = $@"           
                    drop table if exists tmp_lista_dados_frequencia;
                    create temporary table tmp_lista_dados_frequencia as
                    select a.id aula_id,
                           pe.id periodo_id,
                      	   pe.periodo_inicio,
                      	   pe.periodo_fim,
                      	   pe.bimestre,
                      	   rfa.codigo_aluno,
                      	   a.disciplina_id, 
                      	   rfa.valor,  	
                      	   rfa.criado_em,
                      	   row_number() over (partition by a.id, rfa.codigo_aluno, rfa.numero_aula order by rfa.id) sequencia
                      	from registro_frequencia_aluno rfa 
                      		inner join aula a 
                      			on rfa.aula_id = a.id
                      		inner join periodo_escolar pe 
                      			on a.tipo_calendario_id = pe.tipo_calendario_id
                    where not rfa.excluido and
                      	  not a.excluido and
                      	  rfa.codigo_aluno = any(@codigoAlunos) and
                    	  a.turma_id = any(@turmasId) and
                          @dataAula::date between pe.periodo_inicio and pe.periodo_fim and
                          a.data_aula::date between pe.periodo_inicio and pe.periodo_fim;
                    
                    select {(somenteAusencias ? string.Empty : "count(0) filter (where tmp.valor = 1) TotalPresencas,")}
                    	   count(0) filter (where tmp.valor = 2) TotalAusencias,
                    	   {(somenteAusencias ? string.Empty : "count(0) filter (where tmp.valor = 3) TotalRemotos,")}
                    	   tmp.periodo_id as PeriodoEscolarId,
                    	   tmp.periodo_inicio as PeriodoInicio,
                    	   tmp.periodo_fim as PeriodoFim,
                    	   tmp.bimestre,
                           tmp.codigo_aluno as AlunoCodigo,
                           tmp.disciplina_id as ComponenteCurricularId
                    	from tmp_lista_dados_frequencia tmp
                    where tmp.sequencia = 1	                               
                    group by tmp.periodo_id,
                        	 tmp.periodo_inicio,
                        	 tmp.periodo_fim,
                        	 tmp.bimestre,
                        	 tmp.codigo_aluno,
                        	 tmp.disciplina_id;";

            return await database.Conexao.QueryAsync<RegistroFrequenciaPorDisciplinaAlunoDto>(query, new { dataAula, codigoAlunos, turmasId });
        }
    }
}
