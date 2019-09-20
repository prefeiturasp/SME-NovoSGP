using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotificacao : RepositorioBase<Notificacao>, IRepositorioNotificacao
    {
        public RepositorioNotificacao(ISgpContext conexao) : base(conexao)
        {
        }

        public IEnumerable<Notificacao> Obter(string dreId, string ueId, int statusId,
            string turmaId, string usuarioRf, int tipoId, int categoriaId, string titulo, long codigo, int anoLetivo)
        {
            var query = new StringBuilder();

            query.AppendLine("select n.* from notificacao n");
            query.AppendLine("left join usuario u");
            query.AppendLine("on n.usuario_id = u.id");

            query.AppendLine("where excluida = false ");

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
                titulo = $"%{titulo}%";
                query.AppendLine("and lower(f_unaccent(n.titulo)) LIKE @titulo ");
            }

            query.AppendLine("order by id desc");

            return database.Conexao.Query<Notificacao>(query.ToString(), new { dreId, ueId, turmaId, statusId, tipoId, usuarioRf, categoriaId, titulo, codigo, anoLetivo });
        }

        //public (Notificacao, string) ObterDetalhePorId(long idNotificacao)
        //{
        //    var query = new StringBuilder();

        //    query.AppendLine("select n.*, u.* from notificacao n");
        //    query.AppendLine("where excluida = false ");
        //    query.AppendLine("and n.id = @idNotificacao; ");

        //    //query.AppendLine("select wfn.observacao from wf_aprovacao_nivel wfn");
        //    //query.AppendLine("inner join wf_aprovacao_nivel_notificacao wfnn");
        //    //query.AppendLine("on  wfn.id = wfnn.wf_aprovacao_nivel_id");
        //    //query.AppendLine("where wfnn.notificacao_id = @idNotificacao");

        //    //using (var multi = database.Conexao.QueryMultiple(query.ToString(), new { idNotificacao }))
        //    //{
        //    //    var notificacao = multi.Read<Notificacao>().First();
        //    //    var observacao = multi.Read<string>().First();

        //    //    return (notificacao, observacao);
        //    //}
        //}
        public override Notificacao ObterPorId(long id)
        {
            var query = new StringBuilder();

            query.AppendLine("select n.*, wan.* from notificacao n");
            query.AppendLine("left join wf_aprovacao_nivel_notificacao wann");
            query.AppendLine("on wann.notificacao_id = n.id");
            query.AppendLine("join wf_aprovacao_nivel wan");
            query.AppendLine("on wan.id = wann.wf_aprovacao_nivel_id");

            query.AppendLine("where excluida = false ");
            query.AppendLine("and n.id = @id ");

            return database.Conexao.Query<Notificacao>(query.ToString(), new { id })
                .FirstOrDefault();
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