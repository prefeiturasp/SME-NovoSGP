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
            Map(a => a.PlanoAEEVersaoId).ToColumn("plano_aee_versao_id");
        }
    }
}
