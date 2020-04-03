using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotificacao : RepositorioBase<Notificacao>, IRepositorioNotificacao
    {
        public RepositorioNotificacao(ISgpContext conexao) : base(conexao)
        {
        }

        #region Obter paginado

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

        public async Task<PaginacaoResultadoDto<Notificacao>> Obter(string dreId, string ueId, int statusId,
            string turmaId, string usuarioRf, int tipoId, int categoriaId, string titulo, long codigo, int anoLetivo, Paginacao paginacao)
        {
            var query = new StringBuilder();

            MontaQueryObterCompleta(dreId, ueId, statusId, turmaId, usuarioRf, tipoId, categoriaId, titulo, codigo, anoLetivo, paginacao, query,
                paginacao.QuantidadeRegistros, paginacao.QuantidadeRegistrosIgnorados);

            MontaQueryObterCount(dreId, ueId, statusId, turmaId, usuarioRf, tipoId, categoriaId, titulo, codigo, anoLetivo, query);

            var retornoPaginado = new PaginacaoResultadoDto<Notificacao>();

            if (!string.IsNullOrEmpty(titulo))
            {
                titulo = $"%{titulo}%";
            }

            using (var multi = await database.Conexao.QueryMultipleAsync(query.ToString(), new
            {
                dreId,
                ueId,
                turmaId,
                statusId,
                tipoId,
                usuarioRf,
                categoriaId,
                titulo,
                codigo,
                anoLetivo
            }))
            {
                retornoPaginado.Items = multi.Read<Notificacao>().ToList();
                retornoPaginado.TotalRegistros = multi.ReadFirst<int>();
            }

            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);
            return retornoPaginado;
        }

        private static void MontaFiltrosObter(string dreId, string ueId, int statusId, string turmaId, string usuarioRf, int tipoId, int categoriaId, string titulo, long codigo, int anoLetivo, StringBuilder query)
        {
            query.AppendLine("where excluida = false");

            if (!string.IsNullOrEmpty(dreId))
                query.AppendLine("and n.dre_id = @dreId");

            if (!string.IsNullOrEmpty(ueId))
                query.AppendLine("and n.ue_id = @ueId");

            if (!string.IsNullOrEmpty(turmaId))
                query.AppendLine("and n.turma_id = @turmaId");

            if (statusId > 0)
                query.AppendLine("and n.status = @statusId");

            if (tipoId > 0)
                query.AppendLine("and n.tipo = @tipoId");

            if (!string.IsNullOrEmpty(usuarioRf))
                query.AppendLine("and u.rf_codigo = @usuarioRf");

            if (categoriaId > 0)
                query.AppendLine("and n.categoria = @categoriaId");

            if (codigo > 0)
                query.AppendLine("and n.codigo = @codigo");

            if (anoLetivo > 0)
                query.AppendLine("and EXTRACT(year FROM n.criado_em) = @anoLetivo");

            if (!string.IsNullOrEmpty(titulo))
            {
                query.AppendLine("and lower(f_unaccent(n.titulo)) LIKE  @titulo");
            }
        }

        private static void MontaQueryObterCabecalho(StringBuilder query, bool EhParaCount)
        {
            query.Append(";");

            if (EhParaCount)
                query.AppendLine("select count(n.*) from notificacao n");
            else query.AppendLine("select n.* from notificacao n");

            query.AppendLine("left join usuario u");
            query.AppendLine("on n.usuario_id = u.id");
        }

        private static void MontaQueryObterCompleta(string dreId, string ueId, int statusId, string turmaId, string usuarioRf, int tipoId, int categoriaId, string titulo,
            long codigo, int anoLetivo, Paginacao paginacao, StringBuilder query, int quantidadeRegistros, int quantidadeRegistrosIgnorados)
        {
            MontaQueryObterCabecalho(query, false);
            MontaFiltrosObter(dreId, ueId, statusId, turmaId, usuarioRf, tipoId, categoriaId, titulo, codigo, anoLetivo, query);
            query.AppendLine("order by id desc");

            if (paginacao.QuantidadeRegistros != 0)
                query.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", quantidadeRegistrosIgnorados, quantidadeRegistros);
        }

        private static void MontaQueryObterCount(string dreId, string ueId, int statusId, string turmaId, string usuarioRf, int tipoId, int categoriaId, string titulo,
            long codigo, int anoLetivo, StringBuilder query)
        {
            MontaQueryObterCabecalho(query, true);
            MontaFiltrosObter(dreId, ueId, statusId, turmaId, usuarioRf, tipoId, categoriaId, titulo, codigo, anoLetivo, query);
        }

        #endregion Obter paginado

        public async Task ExcluirPorIdsAsync(long[] ids)
        {
            await database.Conexao.ExecuteAsync("DELETE FROM NOTIFICACAO WHERE ID = ANY(@ids)", new { ids });
        }

        public IEnumerable<Notificacao> ObterNotificacoesPorAnoLetivoERf(int anoLetivo, string usuarioRf, int limite)
        {
            var query = new StringBuilder();

            query.AppendLine("select n.* from notificacao n");
            query.AppendLine("left join usuario u");
            query.AppendLine("on n.usuario_id = u.id");
            query.AppendLine("where u.rf_codigo = @usuarioRf");
            query.AppendLine("and EXTRACT(year FROM n.criado_em) = @anoLetivo");
            query.AppendLine("and excluida = @excluida");
            query.AppendLine("order by n.status asc, n.criado_em desc");
            query.AppendLine("limit @limite");

            return database.Conexao.Query<Notificacao>(query.ToString(), new { anoLetivo, usuarioRf, limite, excluida = false });
        }

        public override Notificacao ObterPorId(long id)
        {
            var query = new StringBuilder();

            query.AppendLine("select n.*, wan.*, u.* from notificacao n");
            query.AppendLine("left join wf_aprovacao_nivel_notificacao wann");
            query.AppendLine("on wann.notificacao_id = n.id");
            query.AppendLine("left join wf_aprovacao_nivel wan");
            query.AppendLine("on wan.id = wann.wf_aprovacao_nivel_id");
            query.AppendLine("left join usuario u");
            query.AppendLine("on u.id = n.usuario_id");

            query.AppendLine("where excluida = false ");
            query.AppendLine("and n.id = @id ");

            return database.Conexao.Query<Notificacao, WorkflowAprovacaoNivel, Usuario, Notificacao>(query.ToString(),
                (notificacao, workflowNivel, usuario) =>
                {
                    notificacao.WorkflowAprovacaoNivel = workflowNivel;
                    notificacao.Usuario = usuario;
                    notificacao.UsuarioId = usuario.Id;

                    return notificacao;
                }, param: new { id }).FirstOrDefault();
        }

        public int ObterQuantidadeNotificacoesNaoLidasPorAnoLetivoERf(int anoLetivo, string usuarioRf)
        {
            var query = new StringBuilder();

            query.AppendLine("select count(*) from notificacao n");
            query.AppendLine("left join usuario u");
            query.AppendLine("on n.usuario_id = u.id");
            query.AppendLine("where u.rf_codigo = @usuarioRf");
            query.AppendLine("and excluida = @excluida");
            query.AppendLine("and n.status = @naoLida");
            query.AppendLine("and EXTRACT(year FROM n.criado_em) = @anoLetivo");

            return database.Conexao.QueryFirst<int>(query.ToString(), new { anoLetivo, usuarioRf, excluida = false, naoLida = (int)NotificacaoStatus.Pendente });
        }

        public long ObterUltimoCodigoPorAno(int ano)
        {
            var query = new StringBuilder();

            query.AppendLine("SELECT n.codigo");
            query.AppendLine("FROM notificacao n");
            query.AppendLine("where EXTRACT(year FROM n.criado_em) = @ano");
            query.AppendLine("order by codigo desc");
            query.AppendLine("limit 1");

            return database.Conexao.Query<int>(query.ToString(), new { ano })
                .FirstOrDefault();
        }
    }
}