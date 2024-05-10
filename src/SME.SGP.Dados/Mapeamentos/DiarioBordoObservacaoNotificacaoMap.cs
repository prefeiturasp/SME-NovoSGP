using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class DiarioBordoObservacaoNotificacaoMap : DommelEntityMap<DiarioBordoObservacaoNotificacao>
    {
        public DiarioBordoObservacaoNotificacaoMap()
        {
            ToTable("diario_bordo_observacao_notificacao");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.IdObservacao).ToColumn("observacao_id");
            Map(c => c.IdNotificacao).ToColumn("notificacao_id");
        }
    }    
}
