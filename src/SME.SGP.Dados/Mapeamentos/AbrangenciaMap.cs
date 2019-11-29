using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AbrangenciaMap : DommelEntityMap<Abrangencia>
    {
        public AbrangenciaMap()
        {
            ToTable("abrangencia");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.Perfil).ToColumn("perfil");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.UsuarioId).ToColumn("usuario_id");
        }
    }
}
