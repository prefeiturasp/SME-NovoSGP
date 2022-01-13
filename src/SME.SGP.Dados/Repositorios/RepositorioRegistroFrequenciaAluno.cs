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

        public async Task RemoverPorRegistroFrequenciaId(long registroFrequenciaId)
        {
            await database.Conexao.ExecuteAsync("DELETE FROM registro_frequencia_aluno WHERE registro_frequencia_id = @registroFrequenciaId", 
                new { registroFrequenciaId });
        }

        public async Task RemoverPorRegistroFrequenciaIdENumeroAula(long registroFrequenciaId, int numeroAula, string codigoAluno)
        {
            await database.Conexao.ExecuteAsync("DELETE FROM registro_frequencia_aluno WHERE registro_frequencia_id = @registroFrequenciaId AND numero_aula = @numeroAula AND codigo_aluno = @codigoAluno",
                new { registroFrequenciaId, numeroAula, codigoAluno });
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
    }
}
