using System;
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

            using (var writer = ((NpgsqlConnection) database.Conexao).BeginBinaryImport(sql))
            {
                foreach (var compensacao in registros)
                {
                    writer.StartRow();
                    writer.Write(compensacao.CompensacaoAusenciaId, NpgsqlDbType.Bigint);
                    writer.Write(compensacao.CodigoAluno, NpgsqlDbType.Varchar);
                    writer.Write(compensacao.QuantidadeFaltasCompensadas, NpgsqlDbType.Integer);
                    writer.Write(compensacao.Notificado);
                    writer.Write(compensacao.CriadoPor ?? usuarioLogado.Nome);
                    writer.Write(compensacao.CriadoRF ?? usuarioLogado.Login);
                    writer.Write(compensacao.CriadoEm);
                }

                writer.Complete();
            }

            return await Task.FromResult(true);
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