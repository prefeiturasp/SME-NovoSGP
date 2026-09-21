using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AcompanhamentoAlunoSemestreMap : BaseMap<AcompanhamentoAlunoSemestre>
    {
        public AcompanhamentoAlunoSemestreMap()
        {
            ToTable("acompanhamento_aluno_semestre");
            Map(nameof(AcompanhamentoAlunoSemestre.AcompanhamentoAlunoId), "acompanhamento_aluno_id");
            Map(nameof(AcompanhamentoAlunoSemestre.Semestre), "semestre");
            Map(nameof(AcompanhamentoAlunoSemestre.Observacoes), "observacoes");
            Map(nameof(AcompanhamentoAlunoSemestre.PercursoIndividual), "percurso_individual");
            Map(nameof(AcompanhamentoAlunoSemestre.Excluido), "excluido");
        }
    }
}