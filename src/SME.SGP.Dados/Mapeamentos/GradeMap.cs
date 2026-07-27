using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class GradeMap : BaseEntityMap<Grade>
    {
        public GradeMap()
        {
            ToTable("grade");
            Map(nameof(Grade.Nome), "nome");
        }
    }
}