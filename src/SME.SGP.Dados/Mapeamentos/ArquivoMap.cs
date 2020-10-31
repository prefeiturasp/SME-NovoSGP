using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ArquivoMap : BaseMap<Arquivo>
    {
        public ArquivoMap()
        {
            ToTable("arquivo");
            Map(a => a.NomeFisico).ToColumn("nome_fisico");
        }
    }
}
