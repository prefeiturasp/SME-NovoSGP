using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotificacao : RepositorioBase<Notificacao>, IRepositorioNotificacao
    {
        public RepositorioNotificacao(ISgpContext conexao) : base(conexao)
        {
        }

        public override void Remover(long id)
            => Remover(ObterPorId(id));

        public override void Remover(Notificacao entidade)
        {
            if (entidade != null)
            {
                entidade.Excluida = true;
                Salvar(entidade);
            }
        }

        public async Task ExcluirPorIdsAsync(long[] ids)
        {
            await database.Conexao.ExecuteAsync("DELETE FROM NOTIFICACAO WHERE ID = ANY(@ids)", new { ids });
        }
        public async Task ExcluirLogicamentePorIdsAsync(long[] ids)
        {
            var query = @"UPDATE NOTIFICACAO SET 
                            EXCLUIDA = true, 
                            ALTERADO_EM = @dataAlteracao, 
                            ALTERADO_POR = 'Sistema'
                         WHERE ID = ANY(@ids)";

            await database.Conexao.ExecuteAsync(query, new { ids, dataAlteracao = DateTime.Now });
        }
        
        public override async Task<long> RemoverLogico(long id, string coluna = null)
        {
            var columName = coluna ?? "id";

            var query = $@"update notificacao
                            set excluida = true
                              , alterado_por = @alteradoPor
                              , alterado_rf = @alteradoRF 
                              , alterado_em = @alteradoEm
                        where {columName}=@id RETURNING id";

            return await database.Conexao.ExecuteScalarAsync<long>(query
                , new
                {
                    id,
                    alteradoPor = database.UsuarioLogadoNomeCompleto,
                    alteradoRF = database.UsuarioLogadoRF,
                    alteradoEm = DateTime.Now
                });
        }

        public async Task ExcluirPeloSistemaAsync(long[] ids)
        {
            var sql = "update notificacao set excluida = true, alterado_por = @alteradoPor, alterado_em = @alteradoEm, alterado_rf = @alteradoRf where id = any(@ids)";
            await database.Conexao.ExecuteAsync(sql, new { ids, alteradoPor = "Sistema", alteradoEm = DateTime.Now, alteradoRf = "Sistema" });
        }
    }
}
