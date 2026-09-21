using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoMatriculaTurmaMap : SimpleMap<ConsolidacaoMatriculaTurma>
    {
        public ConsolidacaoMatriculaTurmaMap()
        {
            ToTable("consolidacao_matricula_turma");
            Map(nameof(ConsolidacaoMatriculaTurma.TurmaId), "turma_id");
            Map(nameof(ConsolidacaoMatriculaTurma.Quantidade), "quantidade");
        }
    }
}