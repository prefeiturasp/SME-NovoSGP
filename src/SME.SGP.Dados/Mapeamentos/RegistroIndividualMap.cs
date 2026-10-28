using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class RegistroIndividualMap : BaseMap<RegistroIndividual>
    {
        public RegistroIndividualMap()
        {
            ToTable("registro_individual");
            Map(nameof(RegistroIndividual.TurmaId), "turma_id");
            Map(nameof(RegistroIndividual.AlunoCodigo), "aluno_codigo");
            Map(nameof(RegistroIndividual.ComponenteCurricularId), "componente_curricular_id");
            Map(nameof(RegistroIndividual.DataRegistro), "data_registro");
            Map(nameof(RegistroIndividual.Registro), "registro");
            Map(nameof(RegistroIndividual.Migrado), "migrado");
            Map(nameof(RegistroIndividual.Excluido), "excluido");
        }
    }
}