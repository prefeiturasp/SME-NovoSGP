using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanoAnualMap : BaseEntityMap<PlanoAnual>
    {
        public PlanoAnualMap()
        {
            ToTable("plano_anual");
            Map(nameof(PlanoAnual.Ano), "ano");
            Map(nameof(PlanoAnual.Bimestre), "bimestre");
            Map(nameof(PlanoAnual.ComponenteCurricularEolId), "componente_curricular_eol_id");
            Map(nameof(PlanoAnual.Descricao), "descricao");
            Map(nameof(PlanoAnual.EscolaId), "escola_id");
            Map(nameof(PlanoAnual.Migrado), "migrado");
            Map(nameof(PlanoAnual.TurmaId), "turma_id");
            Map(nameof(PlanoAnual.ObjetivosAprendizagemOpcionais), "objetivos_opcionais");
        }
    }
}