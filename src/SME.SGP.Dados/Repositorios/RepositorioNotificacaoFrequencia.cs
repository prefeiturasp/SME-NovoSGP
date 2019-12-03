using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotificacaoFrequencia : RepositorioBase<NotificacaoFrequencia>, IRepositorioNotificacaoFrequencia
    {
        public RepositorioNotificacaoFrequencia(ISgpContext database) : base(database)
        {
        }

        public bool UsuarioNotificado(long usuarioId, TipoNotificacaoFrequencia tipo)
        {
            var query = @"select 0 
                          from notificacao_frequencia f
                         inner join notificacao n on n.codigo = f.notificacao_codigo
                         where n.usuario_id = @usuarioId
                           and f.tipo = @tipo";

            return database.Conexao.Query<int>(query, new { usuarioId, tipo }).Any();
        }
    }
}
