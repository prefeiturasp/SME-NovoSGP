using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class QuestionarioMap : BaseMap<Questionario>
    {
        public QuestionarioMap()
        {
            ToTable("questionario");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Tipo).ToColumn("tipo");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
