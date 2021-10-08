using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AcompanhamentoAlunoSemestreMap : BaseMap<AcompanhamentoAlunoSemestre>
    {
        public AcompanhamentoAlunoSemestreMap()
        {
            ToTable("acompanhamento_aluno_semestre");
            Map(c => c.AcompanhamentoAlunoId).ToColumn("acompanhamento_aluno_id");
            Map(c => c.Semestre).ToColumn("semestre");
            Map(c => c.Observacoes).ToColumn("observacoes");
            Map(c => c.PercursoIndividual).ToColumn("percurso_individual");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
