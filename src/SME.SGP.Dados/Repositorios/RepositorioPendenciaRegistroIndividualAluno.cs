using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaRegistroIndividualAluno : IRepositorioPendenciaRegistroIndividualAluno
    {
        private readonly ISgpContext database;

        public RepositorioPendenciaRegistroIndividualAluno(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> SalvarAsync(PendenciaRegistroIndividualAluno entidade)
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
    }
}