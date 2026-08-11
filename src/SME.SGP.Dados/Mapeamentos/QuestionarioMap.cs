using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class QuestionarioMap : BaseMap<Questionario>
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