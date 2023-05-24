using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotificacao : RepositorioBase<Notificacao>, IRepositorioNotificacao
    {
        public RepositorioNotificacao(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
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

        public async Task<IEnumerable<NotificacaoBasicaDto>> ObterNotificacoesPorRfAsync(string usuarioRf, int limite = 5)
        {
            var sql = @"select
	                        n.id,
	                        n.categoria,
	                        n.codigo ,
	                        n.criado_em as Data,
	                        n.mensagem as DescricaoStatus,
	                        n.status,
	                        n.tipo,
	                        n.titulo
                        from
	                        notificacao n
                        left join usuario u on
	                        n.usuario_id = u.id
                        where
	                        (u.rf_codigo = @usuarioRf or u.login = @usuarioRf)
	                        and not excluida
                        order by
	                        n.status asc,
	                        n.criado_em desc
                        limit @limite";
            return await database.Conexao.QueryAsync<NotificacaoBasicaDto>(sql, new {usuarioRf, limite });
        }

        public async Task AtualizarMensagemPorWorkFlowAprovacao(long[] ids, string mensagem)
        {
            var query = @"UPDATE NOTIFICACAO SET 
                            MENSAGEM = @mensagem, 
                            ALTERADO_EM = @alteradoEm, 
                            ALTERADO_POR = @alteradoPor,
                            ALTERADO_RF = @alteradoRf
                         WHERE ID = any(@ids)";

           await database.Conexao.ExecuteAsync(query, new { ids, mensagem, alteradoPor = "Sistema", alteradoEm = DateTimeExtension.HorarioBrasilia(), alteradoRf = "Sistema" });
        }

        public async Task<long[]> ObterIdsAsync(string turmaCodigo, NotificacaoCategoria categoria, NotificacaoTipo tipo, int ano)
        {
            var sql = @"select
	                        n.id
                        from
	                        notificacao n
                        where not n.excluida
                        and n.turma_id = @turmaCodigo
                        and n.categoria = @categoria
                        and n.tipo = @tipo
                        and n.ano = @ano ";
            var notificacoes = await database.Conexao.QueryAsync<long>(sql, new { turmaCodigo, categoria = (int)categoria, tipo = (int)tipo, ano });
            return notificacoes.ToArray();
        }
    }
}
