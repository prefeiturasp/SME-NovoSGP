﻿using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
   public class SinteseValorMap : BaseMap<Sintese>
    {
        public SinteseValorMap()
        {
            ToTable("sintese_valores");
            Map(c => c.Aprovado).ToColumn("aprovado");
            Map(c => c.Ativo).ToColumn("ativo");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.FimVigencia).ToColumn("fim_vigencia");
            Map(c => c.InicioVigencia).ToColumn("inicio_vigencia");
            Map(c => c.Valor).ToColumn("valor");
        }
    }
}
