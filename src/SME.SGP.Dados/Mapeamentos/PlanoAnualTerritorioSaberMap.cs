using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    class PlanoAnualTerritorioSaberMap : BaseMap<PlanoAnualTerritorioSaber>
    {
        public PlanoAnualTerritorioSaberMap()
        {
            ToTable("plano_anual_territorio_saber");
            Map(c => c.Ano).ToColumn("ano");
            Map(c => c.Bimestre).ToColumn("bimestre");
            Map(c => c.TerritorioExperienciaId).ToColumn("territorio_experiencia_id");
            Map(c => c.Desenvolvimento).ToColumn("desenvolvimento");
            Map(c => c.Reflexao).ToColumn("reflexao");
            Map(c => c.EscolaId).ToColumn("escola_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
        }
    }
}
