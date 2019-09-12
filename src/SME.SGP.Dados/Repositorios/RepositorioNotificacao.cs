using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotificacao : RepositorioBase<Notificacao>, IRepositorioNotificacao
    {
        public RepositorioNotificacao(ISgpContext conexao) : base(conexao)
        {
        }

        public IEnumerable<Notificacao> ObterPorDreOuEscolaOuStatusOuTurmoOuUsuarioOuTipo(string dreId, string escolaId, int statusId,
            string turmaId, string usuarioRf, int tipoId, int categoriaId)
        {
            var query = new StringBuilder();

            

            query.AppendLine("select n.* from notificacao n");
            query.AppendLine("left join usuario u");
            query.AppendLine("on n.usuario_id = u.id");
            query.AppendLine("where 1=1");

            if (!string.IsNullOrEmpty(dreId))
                query.AppendLine("and n.dre_id = @dreId");

            if (!string.IsNullOrEmpty(escolaId))
                query.AppendLine("and n.escola_id = @escolaId");

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

            return database.Conexao.Query<Notificacao>(query.ToString(), new { dreId, escolaId, turmaId, statusId, tipoId, usuarioRf, categoriaId });
        }
    }
}