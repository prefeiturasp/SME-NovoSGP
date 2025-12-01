using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRegistroFrequenciaAluno : RepositorioBase<RegistroFrequenciaAluno>, IRepositorioRegistroFrequenciaAluno
    {
        public RepositorioRegistroFrequenciaAluno(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        { }

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
            var query = @"delete from compensacao_ausencia_aluno_aula where registro_frequencia_aluno_id = any(@idsParaExcluir);
                          delete from registro_frequencia_aluno where id = any(@idsParaExcluir);";

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

            await using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
            {
                foreach (var frequencia in registros)
                {
                    await writer.StartRowAsync();

                    await writer.WriteAsync(frequencia.Valor, NpgsqlDbType.Bigint);
                    await writer.WriteAsync(frequencia.CodigoAluno);
                    await writer.WriteAsync(frequencia.NumeroAula);
                    await writer.WriteAsync(frequencia.RegistroFrequenciaId);
                    await writer.WriteAsync(frequencia.CriadoEm);

                    await writer.WriteAsync(log ? database.UsuarioLogadoNomeCompleto : frequencia.CriadoPor);
                    await writer.WriteAsync(log ? database.UsuarioLogadoRF : frequencia.CriadoRF);

                    await writer.WriteAsync(frequencia.AulaId);
                }

                await writer.CompleteAsync();
            }

            return true;
        }

        public async Task ExcluirRegistroFrequenciaAlunoPorAulaECodigosAlunos(long aulaId, string[] codigosAlunos)
        {
            var query = @"DELETE from registro_frequencia_aluno WHERE NOT excluido AND aula_id = @aulaId and codigo_aluno = any(@codigosAlunos)";

            await database.Conexao.ExecuteAsync(query, new { aulaId, codigosAlunos });
        }
    }
}
