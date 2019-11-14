using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class GradeMap : BaseMap<Grade>
    {
        public GradeMap()
        {
            ToTable("grade");
            Map(a => a.Nome).ToColumn("nome");
        }
    }
}
