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
            Map(a => a.NotificacaoId).ToColumn("notificacao_id");
            Map(a => a.AulaId).ToColumn("aula_id");
        }
    }
}
