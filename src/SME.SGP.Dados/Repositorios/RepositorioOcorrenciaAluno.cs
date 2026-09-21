using Npgsql;
using NpgsqlTypes; // Adicionado para NpgsqlDbType
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioOcorrenciaAluno : IRepositorioOcorrenciaAluno
    {
        private readonly ISgpContext database;
        private readonly IServicoAuditoria servicoAuditoria;

        public RepositorioOcorrenciaAluno(ISgpContext database, IServicoAuditoria servicoAuditoria)
        {
            this.database = database;
            this.servicoAuditoria = servicoAuditoria ?? throw new ArgumentNullException(nameof(servicoAuditoria));
        }

        public async Task ExcluirAsync(IEnumerable<long> idsOcorrenciasAlunos)
        {
            if (!idsOcorrenciasAlunos?.Any() ?? true) return;

            const string sql = "delete from ocorrencia_aluno where id = any(@idsOcorrenciasAlunos)";
            await database.Conexao.ExecuteAsync(sql, new { idsOcorrenciasAlunos = idsOcorrenciasAlunos.ToList() });
            await AuditarAsync(idsOcorrenciasAlunos, "E");
        }

        public async Task<long> SalvarAsync(OcorrenciaAluno entidade)
        {
            if (entidade.Id > 0)
            {
                await database.Conexao.UpdateMappedAsync(entidade);
                await AuditarAsync(entidade.Id, "A");
            }
            else
            {
                entidade.Id = (await database.Conexao.InsertMappedAsync(entidade));
                await AuditarAsync(entidade.Id, "I");
            }

            return entidade.Id;
        }

        private async Task AuditarAsync(long identificador, string acao)
        {
            var auditoria = new Auditoria()
            {
                Data = DateTimeExtension.HorarioBrasilia(),
                Entidade = nameof(OcorrenciaAluno).ToLower(),
                Chave = identificador,
                Usuario = database.UsuarioLogadoNomeCompleto,
                RF = database.UsuarioLogadoRF,
                Acao = acao
            };

            await servicoAuditoria.Auditar(auditoria);

        }
        public async Task<IEnumerable<string>> ObterAlunosPorOcorrencia(long ocorrenciaId)
        {
            const string query = @"select
                            oa.codigo_aluno
                        from
                            ocorrencia o
                        inner join ocorrencia_tipo ot on ot.id = o.ocorrencia_tipo_id 
                        inner join ocorrencia_aluno oa on oa.ocorrencia_id = o.id
                        where not o.excluido and o.id = @ocorrenciaId ";

            return await database.Conexao.QueryAsync<string>(query.ToString(), new { ocorrenciaId });
        }

        private async Task AuditarAsync(IEnumerable<long> identificadores, string acao)
        {
            const string sql = @"copy auditoria (
                                        data,
                                        entidade,
                                        chave,
                                        usuario,
                                        rf,
                                        acao
                                        )
                            from
                            stdin (FORMAT binary)";

            await using var writer = await ((NpgsqlConnection)database.Conexao).BeginBinaryImportAsync(sql);
            foreach (var identificador in identificadores)
            {
                await writer.StartRowAsync();
                await writer.WriteAsync(DateTime.Now, NpgsqlDbType.Timestamp);
                await writer.WriteAsync(nameof(OcorrenciaAluno).ToLower(), NpgsqlDbType.Varchar);
                await writer.WriteAsync(identificador, NpgsqlDbType.Bigint);
                await writer.WriteAsync(database.UsuarioLogadoNomeCompleto, NpgsqlDbType.Varchar);
                await writer.WriteAsync(database.UsuarioLogadoRF, NpgsqlDbType.Varchar);
                await writer.WriteAsync(acao, NpgsqlDbType.Varchar);
            }
            await writer.CompleteAsync();
        }
    }
}