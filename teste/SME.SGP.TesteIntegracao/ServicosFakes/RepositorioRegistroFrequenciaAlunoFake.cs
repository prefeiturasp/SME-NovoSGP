using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dados;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class RepositorioRegistroFrequenciaAlunoFake : RepositorioBase<RegistroFrequenciaAluno>, IRepositorioRegistroFrequenciaAluno
    {
        public RepositorioRegistroFrequenciaAlunoFake(ISgpContext sgpContext,IServicoAuditoria servicoAuditoria) : base(sgpContext, servicoAuditoria)
        {}

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
            foreach (var entidade in registros)
            {
                if (entidade.Id > 0)
                {                
                    entidade.AlteradoEm = DateTimeExtension.HorarioBrasilia();
                    entidade.AlteradoPor = database.UsuarioLogadoNomeCompleto;
                    entidade.AlteradoRF = database.UsuarioLogadoRF;
                    await database.Conexao.UpdateAsync(entidade);
                    await AuditarAsync(entidade.Id, "A");
                }
                else
                {
                    entidade.CriadoPor = database.UsuarioLogadoNomeCompleto;
                    entidade.CriadoRF = database.UsuarioLogadoRF;
                    entidade.Id = (long)(await database.Conexao.InsertAsync(entidade));
                    await AuditarAsync(entidade.Id, "I");
                }
            }

            return true; 
        }

        public async Task ExcluirVariosLogicamente(long[] idsParaExcluir)
        {
            var query = "update registro_frequencia_aluno set excluido = true where id = any(@idsParaExcluir)";

            await database.Conexao.ExecuteAsync(query, new { idsParaExcluir });
        }

        public async Task ExcluirRegistroFrequenciaAlunoPorAulaECodigosAlunos(long aulaId, string[] codigosAlunos)
        {
            var query = @"DELETE FROM registro_frequencia_aluno WHERE NOT excluido AND aula_id = @aulaId and codigo_aluno = any(@codigosAlunos)";

            await database.Conexao.ExecuteAsync(query, new { aulaId, codigosAlunos });
        }
    }
}
