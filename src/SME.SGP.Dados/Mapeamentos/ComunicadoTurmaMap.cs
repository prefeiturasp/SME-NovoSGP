using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComunicadoTurmaMap : BaseMap<ComunicadoTurma>
    {
        public ComunicadoTurmaMap() : base()
        {
            ToTable("comunicado_turma");
            Map(x => x.Id).ToColumn("id");
            Map(c => c.CodigoTurma).ToColumn("turma_codigo");
            Map(c => c.ComunicadoId).ToColumn("comunicado_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
