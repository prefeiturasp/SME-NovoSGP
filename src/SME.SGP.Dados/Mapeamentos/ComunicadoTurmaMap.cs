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
            Map(x => x.CodigoTurma).ToColumn("turma_codigo");
            Map(x => x.ComunicadoId).ToColumn("comunicado_id");
            Map(x => x.Excluido).ToColumn("excluido");
        }
    }
}
