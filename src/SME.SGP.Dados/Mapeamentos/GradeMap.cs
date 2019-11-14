using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class GradeMap : BaseMap<Grade>
    {
        public GradeMap()
        {
            ToTable("grade");
            Map(a => a.Nome).ToColumn("nome");
        }
    }
}