using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanoAnualTerritorioSaberMap : BaseMap<PlanoAnualTerritorioSaber>
    {
        public PlanoAnualTerritorioSaberMap()
        {
            ToTable("plano_anual_territorio_saber");
            Map(nameof(PlanoAnualTerritorioSaber.Ano), "ano");
            Map(nameof(PlanoAnualTerritorioSaber.Bimestre), "bimestre");
            Map(nameof(PlanoAnualTerritorioSaber.TerritorioExperienciaId), "territorio_experiencia_id");
            Map(nameof(PlanoAnualTerritorioSaber.Desenvolvimento), "desenvolvimento");
            Map(nameof(PlanoAnualTerritorioSaber.Reflexao), "reflexao");
            Map(nameof(PlanoAnualTerritorioSaber.EscolaId), "escola_id");
            Map(nameof(PlanoAnualTerritorioSaber.TurmaId), "turma_id");
        }
    }
}