using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AcompanhamentoAlunoSemestreMap : BaseMap<AcompanhamentoAlunoSemestre>
    {
        public AcompanhamentoAlunoSemestreMap()
        {
            ToTable("acompanhamento_aluno_semestre");
            Map(a => a.AcompanhamentoAlunoId).ToColumn("acompanhamento_aluno_id");
            Map(a => a.PercursoIndividual).ToColumn("percurso_individual");
        }
    }
}
