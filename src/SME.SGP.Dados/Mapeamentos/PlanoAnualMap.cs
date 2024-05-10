using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanoAnualMap : BaseMap<PlanoAnual>
    {
        public PlanoAnualMap()
        {
            ToTable("plano_anual");
            Map(c => c.Ano).ToColumn("ano");
            Map(c => c.Bimestre).ToColumn("bimestre");
            Map(c => c.ComponenteCurricularEolId).ToColumn("componente_curricular_eol_id");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.EscolaId).ToColumn("escola_id");
            Map(c => c.Migrado).ToColumn("migrado");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.ObjetivosAprendizagemOpcionais).ToColumn("objetivos_opcionais");
        }
    }
}