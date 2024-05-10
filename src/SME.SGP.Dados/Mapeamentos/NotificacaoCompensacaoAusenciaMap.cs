using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotificacaoCompensacaoAusenciaMap: DommelEntityMap<NotificacaoCompensacaoAusencia>
    {
        public NotificacaoCompensacaoAusenciaMap()
        {
            ToTable("notificacao_compensacao_ausencia");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.NotificacaoId).ToColumn("notificacao_id");
            Map(c => c.CompensacaoAusenciaId).ToColumn("compensacao_ausencia_id");
        }

    }
}
