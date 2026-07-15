using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCompensacaoAusenciaAluno : RepositorioBase<CompensacaoAusenciaAluno>, IRepositorioCompensacaoAusenciaAluno
    {
        public RepositorioCompensacaoAusenciaAluno(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<bool> InserirVarios(IEnumerable<CompensacaoAusenciaAluno> registros, Usuario usuarioLogado)
        {
            var sql = @"copy compensacao_ausencia_aluno (                                         
                                        compensacao_ausencia_id, 
                                        codigo_aluno,
                                        qtd_faltas_compensadas, 
                                        notificado,
                                        criado_por,                                        
                                        criado_rf,
                                        criado_em)
                            from
                            stdin (FORMAT binary)";


            await using var writer = await ((NpgsqlConnection)database.Conexao).BeginBinaryImportAsync(sql);
            foreach (var compensacao in registros)
            {
                await writer.StartRowAsync();
                await writer.WriteAsync(compensacao.CompensacaoAusenciaId, NpgsqlDbType.Bigint);
                await writer.WriteAsync(compensacao.CodigoAluno, NpgsqlDbType.Varchar);
                await writer.WriteAsync(compensacao.QuantidadeFaltasCompensadas, NpgsqlDbType.Integer);
                await writer.WriteAsync(compensacao.Notificado);
                await writer.WriteAsync(compensacao.CriadoPor ?? usuarioLogado.Nome);
                await writer.WriteAsync(compensacao.CriadoRF ?? usuarioLogado.Login);
                await writer.WriteAsync(compensacao.CriadoEm);
            }

            await writer.CompleteAsync();

            return true; 
        }

        public async Task<bool> AlterarQuantidadeCompensacoesPorCompensacaoAlunoId(long compensacaoAusenciaAlunoId, int quantidade)
        {
            var sql = $@"update compensacao_ausencia_aluno set qtd_faltas_compensadas = @quantidade where id = @compensacaoAusenciaAlunoId";

            return await database.Conexao.ExecuteScalarAsync<bool>(sql, new { compensacaoAusenciaAlunoId, quantidade});
        }

        public async Task<bool> ExcluirCompensacaoAusenciaAlunoPorId(long id)
        {
            var sql = $@"delete from compensacao_ausencia_aluno where id = @id";

            return await database.Conexao.ExecuteScalarAsync<bool>(sql, new { id});
        }

        public async Task<bool> AlterarQuantidadeFaltasCompensadasPorId(long compensacoesAlunosAtualizar, int quantidade)
        {
            var sql = $@"update compensacao_ausencia_aluno set qtd_faltas_compensadas = @quantidade where id = @compensacoesAlunosAtualizar";

            return await database.Conexao.ExecuteScalarAsync<bool>(sql, new { compensacoesAlunosAtualizar, quantidade});
        }
    }
}