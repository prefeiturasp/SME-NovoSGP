using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class DisciplinaPlanoMap : BaseEntityMap<DisciplinaPlano>
    {
        public DisciplinaPlanoMap()
        {
            ToTable("disciplina_plano");
            Map(nameof(DisciplinaPlano.DisciplinaId), "disciplina_id");
            Map(nameof(DisciplinaPlano.PlanoId), "plano_id");
        }
    }
}