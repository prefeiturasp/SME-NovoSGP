using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanoAnualMap : BaseMap<PlanoAnual>
    {
        public PlanoAnualMap()
        {
            ToTable("plano_anual");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.EscolaId).ToColumn("escola_id");
            Map(c => c.ComponenteCurricularEolId).ToColumn("componente_curricular_eol_id");
        }
    }
}