using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class PlanoAEEReestruturacaoMap : BaseMap<PlanoAEEReestruturacao>
    {
        public PlanoAEEReestruturacaoMap()
        {
            ToTable("plano_aee_reestruturacao");
            Map(c => c.PlanoAEEVersaoId).ToColumn("plano_aee_versao_id");
            Map(c => c.Semestre).ToColumn("semestre");
            Map(c => c.Descricao).ToColumn("descricao");
        }
    }
}
