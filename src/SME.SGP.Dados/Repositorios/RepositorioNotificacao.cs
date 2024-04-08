using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if (entidade.NaoEhNulo())
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

        public async Task<IEnumerable<long>> ObterIdsNotificacoesParaExclusao(int ano, long? diasLidasDeAlerta, long? diasLidasDeAviso, long? diasNaoLidasDeAvisoEAlerta)
        {
            var sql = new StringBuilder();

            if (diasLidasDeAlerta.NaoEhNulo())
                sql.AppendLine(@$"SELECT id 
                                 FROM notificacao n
                                 WHERE n.status = {(int)NotificacaoStatus.Lida} 
                                 AND n.categoria = {(int)NotificacaoCategoria.Alerta} 
                                 AND CAST(TO_CHAR(CURRENT_DATE - (n.criado_em  -  INTERVAL '1 DAY'), 'DD') AS INTEGER) >= @diasLidasDeAlerta 
                                 AND n.ano = @ano");

            if (diasLidasDeAviso.NaoEhNulo())
            {
                if (sql.Length > 0)
                    sql.AppendLine(" UNION");

                sql.AppendLine(@$"SELECT id 
                                    FROM notificacao n
                                    WHERE n.status = {(int)NotificacaoStatus.Lida} 
                                    AND n.categoria = {(int)NotificacaoCategoria.Aviso} 
                                    AND CAST(TO_CHAR(CURRENT_DATE - (n.criado_em  -  INTERVAL '1 DAY'), 'DD') AS INTEGER) >= @diasLidasDeAviso 
                                    AND n.ano = @ano");
            }

            if (diasNaoLidasDeAvisoEAlerta.NaoEhNulo())
            {
                if (sql.Length > 0)
                    sql.AppendLine(" UNION");

                sql.AppendLine(@$"SELECT id 
                                  FROM notificacao n
                                  WHERE n.status = {(int)NotificacaoStatus.Pendente} 
                                  AND n.categoria in({(int)NotificacaoCategoria.Alerta}, {(int)NotificacaoCategoria.Aviso}) 
                                  AND CAST(TO_CHAR(CURRENT_DATE - (n.criado_em  -  INTERVAL '1 DAY'), 'DD') AS INTEGER) >= @diasNaoLidasDeAvisoEAlerta 
                                  AND n.ano = @ano");
            }

            return await database.Conexao.QueryAsync<long>(sql.ToString(), new { ano, diasLidasDeAlerta, diasLidasDeAviso, diasNaoLidasDeAvisoEAlerta });
        }

        public async Task<bool> ExcluirTotalPorIdsAsync(long id)
        {
            var sql = new StringBuilder();

            sql.AppendLine("DELETE FROM carta_intencoes_observacao_notificacao WHERE notificacao_id = @id; ");

            sql.AppendLine("DELETE FROM devolutiva_diario_bordo_notificacao WHERE notificacao_id = @id; ");

            sql.AppendLine("DELETE FROM diario_bordo_observacao_notificacao WHERE notificacao_id = @id; ");

            sql.AppendLine("DELETE FROM fechamento_reabertura_notificacao WHERE notificacao_id = @id; ");

            sql.AppendLine("DELETE FROM notificacao_aula WHERE notificacao_id = @id; ");

            sql.AppendLine("DELETE FROM notificacao_compensacao_ausencia WHERE notificacao_id = @id; ");

            sql.AppendLine("DELETE FROM notificacao_plano_aee WHERE notificacao_id = @id; ");

            sql.AppendLine("DELETE FROM notificacao_plano_aee_observacao WHERE notificacao_id = @id; ");

            sql.AppendLine("DELETE FROM wf_aprovacao_nivel_notificacao WHERE notificacao_id = @id; ");

            sql.AppendLine("DELETE FROM notificacao WHERE id = @id; ");

            return await database.Conexao.ExecuteAsync(sql.ToString(), new { id }) != 0;
        }
    }
}
