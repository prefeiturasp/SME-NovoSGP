using Dapper;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioRegistroFrequenciaAluno : RepositorioBase<RegistroFrequenciaAluno>, IRepositorioRegistroFrequenciaAluno
    {
        private readonly ISgpContextConsultas sgpContextConsultas;

        public RepositorioRegistroFrequenciaAluno(ISgpContext conexao, IServicoAuditoria servicoAuditoria, ISgpContextConsultas sgpContextConsultas) : base(conexao, servicoAuditoria)
        {
            this.sgpContextConsultas = sgpContextConsultas ?? throw new ArgumentNullException(nameof(sgpContextConsultas));
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

        public async Task<IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto>> ObterRegistroFrequenciaAlunosPorAlunosETurmaIdEDataAula(DateTime dataAula, string[] turmasId, IEnumerable<(string codigo, DateTime dataMatricula, DateTime? dataSituacao)> alunos, bool somenteAusencias = false)
        {
            var query = "with lista1 as (";
            var listaAlunos = alunos.ToList();

            for (int i = 0; i < listaAlunos.Count; i++)
            {
                query += $@"
                    select a.id aula_id,
                           pe.id periodo_id,
                      	   pe.periodo_inicio,
                      	   pe.periodo_fim,
                      	   pe.bimestre,
                      	   rfa.codigo_aluno,
                      	   a.disciplina_id, 
                      	   rfa.valor,  	
                      	   rfa.criado_em,
                           rfa.numero_aula,
                           rfa.id registro_frequencia_aluno_id                    	   
                      	from registro_frequencia_aluno rfa 
                      		inner join aula a 
                      			on rfa.aula_id = a.id
                      		inner join periodo_escolar pe 
                      			on a.tipo_calendario_id = pe.tipo_calendario_id
                    where not rfa.excluido and
                      	  not a.excluido and
                      	  rfa.codigo_aluno = '{listaAlunos[i].codigo}' and
                    	  a.turma_id = any(@turmasId) and
                          @dataAula::date between pe.periodo_inicio and pe.periodo_fim and
                          a.data_aula::date between pe.periodo_inicio and pe.periodo_fim and
                          a.data_aula::date >= '{listaAlunos[i].dataMatricula:yyyy-MM-dd}'::date
                          {(listaAlunos[i].dataSituacao.HasValue ? $" and a.data_aula::date < '{listaAlunos[i].dataSituacao.Value:yyyy-MM-dd}'::date" : string.Empty)}";

                query += i + 1 == listaAlunos.Count ? string.Empty : " union ";
            }
                    
            query += $@"), lista2 as (
                    select *,
                           row_number() over (partition by aula_id, codigo_aluno, numero_aula order by registro_frequencia_aluno_id) sequencia
                    from lista1)
                    select {(somenteAusencias ? string.Empty : "count(0) filter (where tmp.valor = 1) TotalPresencas,")}
                    	   count(0) filter (where tmp.valor = 2) TotalAusencias,
                    	   {(somenteAusencias ? string.Empty : "count(0) filter (where tmp.valor = 3) TotalRemotos,")}
                    	   tmp.periodo_id as PeriodoEscolarId,
                    	   tmp.periodo_inicio as PeriodoInicio,
                    	   tmp.periodo_fim as PeriodoFim,
                    	   tmp.bimestre,
                           tmp.codigo_aluno as AlunoCodigo,
                           tmp.disciplina_id as ComponenteCurricularId
                    	from lista2 tmp
                    where tmp.sequencia = 1	                               
                    group by tmp.periodo_id,
                        	 tmp.periodo_inicio,
                        	 tmp.periodo_fim,
                        	 tmp.bimestre,
                        	 tmp.codigo_aluno,
                        	 tmp.disciplina_id;";

            return await sgpContextConsultas.Conexao.QueryAsync<RegistroFrequenciaPorDisciplinaAlunoDto>(query, new { dataAula, turmasId }, commandTimeout: 120);
        }
    }
}
