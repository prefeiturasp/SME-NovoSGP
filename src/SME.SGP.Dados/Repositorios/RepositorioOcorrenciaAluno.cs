using Dapper;
using Npgsql;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioOcorrenciaAluno : IRepositorioOcorrenciaAluno
    {
        private readonly ISgpContext database;

        public RepositorioOcorrenciaAluno(ISgpContext database)
        {
            this.database = database;
        }

        public async Task ExcluirAsync(IEnumerable<long> idsOcorrenciasAlunos)
        {
            if (!idsOcorrenciasAlunos?.Any() ?? true) return;

            var sql = "delete from ocorrencia_aluno where id = any(@idsOcorrenciasAlunos)";
            await database.Conexao.ExecuteAsync(sql, new { idsOcorrenciasAlunos = idsOcorrenciasAlunos.ToList() });
            await AuditarAsync(idsOcorrenciasAlunos, "E");
        }

        public async Task<long> SalvarAsync(OcorrenciaAluno entidade)
        {
            if (entidade.Id > 0)
            {
                await database.Conexao.UpdateAsync(entidade);
                await AuditarAsync(entidade.Id, "A");
            }
            else
            {
                entidade.Id = (long)(await database.Conexao.InsertAsync(entidade));
                await AuditarAsync(entidade.Id, "I");
            }

            return entidade.Id;
        }

        private async Task AuditarAsync(long identificador, string acao)
        {
            await database.Conexao.InsertAsync(new Auditoria()
            {
                Data = DateTime.Now,
                Entidade = nameof(OcorrenciaAluno).ToLower(),
                Chave = identificador,
                Usuario = database.UsuarioLogadoNomeCompleto,
                RF = database.UsuarioLogadoRF,
                Acao = acao
            });
        }
        public async Task<IEnumerable<string>> ObterAlunosPorOcorrencia(long ocorrenciaId)
        {
            string query = @"select
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

            using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
            {
                foreach (var identificador in identificadores)
                {
                    writer.StartRow();
                    writer.Write(DateTime.Now);
                    writer.Write(nameof(OcorrenciaAluno).ToLower());
                    writer.Write(identificador);
                    writer.Write(database.UsuarioLogadoNomeCompleto);
                    writer.Write(database.UsuarioLogadoRF);
                    writer.Write(acao);
                }
                await writer.CompleteAsync();
            }
        }
    }
}