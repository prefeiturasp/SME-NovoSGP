using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class DisciplinaPlanoMap : BaseMap<DisciplinaPlano>
    {
        public DisciplinaPlanoMap()
        {
            ToTable("disciplina_plano");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.PlanoId).ToColumn("plano_id");
        }
    }
}