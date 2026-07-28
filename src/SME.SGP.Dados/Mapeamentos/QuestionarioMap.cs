using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class QuestionarioMap : BaseEntityMap<Questionario>
    {
        public QuestionarioMap()
        {
            ToTable("questionario");
            Map(nameof(Questionario.Nome), "nome");
            Map(nameof(Questionario.Tipo), "tipo");
            Map(nameof(Questionario.Excluido), "excluido");
        }
    }
}