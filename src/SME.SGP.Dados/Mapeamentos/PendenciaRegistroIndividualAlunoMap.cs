using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaRegistroIndividualAlunoMap : SimpleEntityMap<PendenciaRegistroIndividualAluno>
    {
        public PendenciaRegistroIndividualAlunoMap()
        {
            ToTable("pendencia_registro_individual_aluno");
            Map(nameof(PendenciaRegistroIndividualAluno.CodigoAluno), "codigo_aluno");
            Map(nameof(PendenciaRegistroIndividualAluno.PendenciaRegistroIndividualId), "pendencia_registro_individual_id");
            Map(nameof(PendenciaRegistroIndividualAluno.Situacao), "situacao");
        }
    }
}