using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class RegistroIndividualMap : BaseMap<RegistroIndividual>
    {
        public RegistroIndividualMap()
        {
            ToTable("registro_individual");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.ComponenteCurricularId).ToColumn("componente_curricular_id");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
            Map(c => c.DataRegistro).ToColumn("data_registro");
        }
    }
}