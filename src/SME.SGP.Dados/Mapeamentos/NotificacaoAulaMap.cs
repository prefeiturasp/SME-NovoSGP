using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotificacaoAulaMap : DommelEntityMap<NotificacaoAula>
    {
        public NotificacaoAulaMap()
        {
            ToTable("notificacao_aula");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.NotificacaoId).ToColumn("notificacao_id");
            Map(c => c.AulaId).ToColumn("aula_id");
        }
    }
}
