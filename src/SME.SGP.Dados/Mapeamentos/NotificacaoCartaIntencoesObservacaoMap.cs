using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotificacaoCartaIntencoesObservacaoMap : DommelEntityMap<NotificacaoCartaIntencoesObservacao>
    {
        public NotificacaoCartaIntencoesObservacaoMap()
        {
            ToTable("carta_intencoes_observacao_notificacao");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.NotificacaoId).ToColumn("notificacao_id");
            Map(c => c.CartaIntencoesObservacaoId).ToColumn("observacao_id");
        }
    }
}
